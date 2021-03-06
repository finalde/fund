using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using BusinessLayer.Query.Queries;
using Data.Contract;
using Data.Writer;
using FundImporter.Services;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace FundImporter.Bls
{
    public class JsonFundDetail
    {
        public string content { get; set; }
        public int records { get; set; }
        public int pages { get; set; }
        public int curpage { get; set; }
    }
    public interface IFundDetailBl
    {
        Task RunAsync();
    }
    public class FundDetailBl: IFundDetailBl
    {
        private readonly IFundQuery _fundQuery;
        private readonly IConvertService _convertService;
        private readonly IDateWriter<long, FundDetail> _fundDetailWriter;
        private readonly ILogger<FundDetailBl> _logger;
        private readonly IFundDetailQuery _fundDetailQuery;

        public FundDetailBl(IFundQuery fundQuery, IConvertService convertService, IDateWriter<long, FundDetail> fundDetailWriter, ILogger<FundDetailBl> logger, IFundDetailQuery fundDetailQuery)
        {
            _fundQuery = fundQuery;
            _convertService = convertService;
            _fundDetailWriter = fundDetailWriter;
            _logger = logger;
            _fundDetailQuery = fundDetailQuery;
        }

        public async Task RunAsync()
        {
            using var client = new WebClient();
          
            var funds = await _fundQuery.GetValueObjectsAsync();
            var maxDateDic = (await _fundDetailQuery.GetAllValueObjectAsync()).GroupBy(x => x.Fund).ToDictionary(x => x.Key, v => v.Max(y => y.Date));
            funds = funds.Where(x => !maxDateDic.ContainsKey(x.Code)).ToList();
            foreach (var fund in funds)
            {
                try
                {
                    var url = $"https://fundf10.eastmoney.com/F10DataApi.aspx?type=lsjz&code={fund.Code}&page=1&sdate=2001-02-11&edate=2050-01-01&per=20";
                    var str = client.DownloadString(new Uri(url));
                    str = str.Replace("var apidata=", "").Trim().TrimEnd(';');
                    var pageInfo = JsonConvert.DeserializeObject<JsonFundDetail>(str);
                    var list = new List<FundDetail>();
                    var startDate = maxDateDic.ContainsKey(fund.Code) ? maxDateDic[fund.Code] : new DateTime(2000, 1, 1);
                    for (int page = 1; page <= pageInfo.pages; page++)
                    {
                        var pageUrl = $"https://fundf10.eastmoney.com/F10DataApi.aspx?type=lsjz&code={fund.Code}&page={page}&sdate={startDate.ToString("yyyy-MM-dd")}&edate=2050-01-01&per=20";
                        var pageStr = client.DownloadString(new Uri(pageUrl));
                        pageStr = pageStr.Replace("var apidata=", "").Trim().TrimEnd(';');
                        var data = JsonConvert.DeserializeObject<JsonFundDetail>(pageStr);
                        HtmlAgilityPack.HtmlDocument htmlDoc = new HtmlAgilityPack.HtmlDocument();
                        htmlDoc.LoadHtml(data.content);
                        HtmlAgilityPack.HtmlNode table = htmlDoc.DocumentNode.SelectSingleNode("//table");
                        var trs = table.SelectNodes("//tr").Skip(1).ToList();
                        var headers = table.SelectNodes("//tr").First().ChildNodes.Select(x => x.InnerText.Trim()).ToList();
                        foreach (var tr in trs)
                        {
                            var tds = tr.ChildNodes.Select(x => x.InnerText.Trim()).ToList();
                            var date = _convertService.ConvertToDate(tds[0]);
                            if (date <= startDate) continue;
                            try
                            {
                                if (tds.Count == 7)
                                {

                                    var item = new FundDetail
                                    {
                                        Id = 0,
                                        Fund = fund.Code,
                                        Date = _convertService.ConvertToDate(tds[0]),
                                        UnitValue = _convertService.ConvertToNullableDecimal(tds[1]),
                                        CumulativeValue = _convertService.ConvertToNullableDecimal(tds[2]),
                                        DailyIncrease = _convertService.ConvertToNullableDecimal(tds[3]),
                                        CreationStatus = tds[4],
                                        RedemptionStatus = tds[5],
                                        Dividend = tds[6],
                                        Active = true,
                                        EventTime = DateTime.Now,
                                        AuditBy = "",
                                        EventType = "I"
                                    };
                                    list.Add(item);
                                }
                                else if (tds.Count == 6)
                                {
                                    var item = new FundDetail
                                    {
                                        Id = 0,
                                        Fund = fund.Code,
                                        Date = _convertService.ConvertToDate(tds[0]),
                                        PnlPerTenThousandsShare = _convertService.ConvertToNullableDecimal(tds[1]),
                                        AnualizedProfitSevenDays = _convertService.ConvertToNullableDecimal(tds[2]),
                                        CreationStatus = tds[3],
                                        RedemptionStatus = tds[4],
                                        Dividend = tds[5],
                                        Active = true,
                                        EventTime = DateTime.Now,
                                        AuditBy = "",
                                        EventType = "I"
                                    };
                                    list.Add(item);
                                }
                                else
                                {
                                    throw new Exception("not handled");
                                }
                            }
                            catch (Exception ex)
                            {

                            }
                        }
                    }
                    if (list.Any())
                    {
                        await _fundDetailWriter.AddRangeAsync(list, true);
                        _logger.LogInformation($"Saved {list.Count} records for fund {fund.Name}...");
                    }
                }catch(Exception ex)
                {
                    _logger.LogError($"Error occured while saving fund {fund.Name}, {ex.Message}, {ex.StackTrace}");
                }
            }
        }
    }
}
