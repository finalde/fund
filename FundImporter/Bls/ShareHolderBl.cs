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
    public class JsonShareholder
    {
        public string content { get; set; }
        public string summary { get; set; }
    }
    public interface IShareholderBl
    {
        Task RunAsync();
    }
    public class ShareholderBl : IShareholderBl
    {
        private readonly IFundQuery _fundQuery;
        private readonly IDateWriter<long, Shareholder> _shareholderWriter;
        private readonly IShareholderQuery _shareholderQuery;
        private readonly IConvertService _convertService;
        private readonly ILogger<ShareholderBl> _logger;

        public ShareholderBl(IFundQuery fundQuery, IDateWriter<long, Shareholder> shareholderWriter, IShareholderQuery shareholderQuery, IConvertService convertService, ILogger<ShareholderBl> logger)
        {
            _fundQuery = fundQuery;
            _shareholderWriter = shareholderWriter;
            _shareholderQuery = shareholderQuery;
            _convertService = convertService;
            _logger = logger;
        }

        public async Task RunAsync()
        {
            var funds = await _fundQuery.GetValueObjectsAsync();
            HtmlAgilityPack.HtmlDocument htmlDoc = new HtmlAgilityPack.HtmlDocument();
            using var client = new WebClient();
            //var list = new List<Shareholder>();
            foreach (var fund in funds)
            {
                var list = new List<Shareholder>();
                var url = $"http://fundf10.eastmoney.com/FundArchivesDatas.aspx?type=cyrjg&code={fund.Code}&rt=0.06092284056904407";
                var str = client.DownloadString(new Uri(url));
                str = str.Replace("var apidata=", "").Trim().TrimEnd(';');
                var pageInfo = JsonConvert.DeserializeObject<JsonShareholder>(str);
                if (string.IsNullOrEmpty(pageInfo.content)) continue;

                try
                {
                    htmlDoc.LoadHtml(pageInfo.content);
                    var trs = htmlDoc.DocumentNode.SelectNodes("//tr")
                        .Where(x => x.ChildNodes.Count == 5)
                        .Skip(1)
                        .Select(x => x.ChildNodes.Select(y => y.InnerText).ToList())
                        .ToList();
                    foreach (var tr in trs)
                    {
                        var item = new Shareholder
                        {
                            Id = default,
                            Date = _convertService.ConvertToDate(tr[0]),
                            Fund = fund.Code,
                            Institution = _convertService.ConvertToNullableDecimal(tr[1]),
                            Individual = _convertService.ConvertToNullableDecimal(tr[2]),
                            Internal = _convertService.ConvertToNullableDecimal(tr[3]),
                            TotalAmount = _convertService.ConvertToNullableDecimal(tr[4]),
                            Active = true,
                            EventTime = DateTime.Now,
                            AuditBy = "",
                            EventType = "I"
                        };
                        list.Add(item);
                    }
                    await _shareholderWriter.AddRangeAsync(list, true);
                }
                catch (Exception ex)
                {

                }
            }

        }
    }
}
