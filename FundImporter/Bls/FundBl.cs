using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using BusinessLayer.Query.Mappers;
using BusinessLayer.Query.Queries;
using Data.Contract;
using Data.Writer;
using FundImporter.Services;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace FundImporter.Bls
{
    public interface IFundBl
    {
        Task RunAsync();
    }
    public class FundBl: IFundBl
    {
        private readonly IFundQuery _fundQuery;
        private readonly IDateWriter<long, Fund> _fundWriter;
        private readonly ILogger<FundBl> _logger;
        private readonly IConvertService _convertService;

        public FundBl(IFundQuery fundQuery, IDateWriter<long, Fund> fundWriter, ILogger<FundBl> logger, IConvertService convertService)
        {
            _fundQuery = fundQuery ?? throw new ArgumentNullException(nameof(fundQuery));
            _fundWriter = fundWriter ?? throw new ArgumentNullException(nameof(fundWriter));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _convertService = convertService ?? throw new ArgumentNullException(nameof(convertService));
        }

        public async Task RunAsync()
        {
            using var client = new WebClient();
            HtmlAgilityPack.HtmlDocument htmlDoc = new HtmlAgilityPack.HtmlDocument();
            var data = client.DownloadString(new Uri("http://fund.eastmoney.com/js/fundcode_search.js"));
            data = data.Replace("var r = ", "").Trim().TrimEnd(';');
            var jarray = JsonConvert.DeserializeObject<JArray>(data);
            var list = new List<Fund>();
            var propsList = jarray.Select(x => x.Children().Select(y => y.ToString()).ToList()).ToList();
            foreach(var props in propsList)
            {
                try
                {
                    using var innerClient = new WebClient();
                    var fundHtmlUrl = $"http://fund.eastmoney.com/{props[0]}.html";
                    var fundHtml = innerClient.DownloadString(new Uri(fundHtmlUrl));

                    htmlDoc.LoadHtml(fundHtml);
                    var twos = htmlDoc.DocumentNode.SelectNodes("//tr").Where(x => x.ChildNodes.Count == 2)
                        .Select(x => (Text: x.ChildNodes[0].InnerText.Trim(), Value: x.ChildNodes[1].InnerText.Trim()))
                        .ToList();
                    var threes = htmlDoc.DocumentNode.SelectNodes("//tr").Where(x => x.ChildNodes.Count == 3)
                        .Take(2)
                        .SelectMany(x => x.ChildNodes)
                        .Where(x => x.ChildNodes.Count >= 2)
                        .Select(x => (Text: x.ChildNodes[0].InnerText.Replace("：", "").Trim(), Value: x.ChildNodes[1].InnerText.Replace("：", "").Replace(",", "").Trim()))
                        .ToList();
                    var dds = htmlDoc.DocumentNode.SelectNodes("//dd").Where(x => x.ChildNodes.Count == 2)
                        .Select(x => (Text: x.ChildNodes[0].InnerText.Replace("：", "").Trim(), Value: x.ChildNodes[1].InnerText.Replace("：", "").Replace(",", "").Trim()))
                        .ToList();
                    var dic = twos.Concat(dds).Concat(threes).GroupBy(x => x.Text).ToDictionary(x => x.Key, v => v.First().Value);
                    var newItem = new Fund
                    {
                        Id = 0,
                        Date = DateTime.Now.Date,
                        Code = props[0],
                        ShortName = props[1],
                        Name = props[2],
                        Type = props[3],
                        FullName = props[4],
                        SinceInceptionIncrease = dic.ContainsKey("成立来") ? _convertService.ConvertToNullableDecimal(dic["成立来"]) : null,
                        ThreeYearIncrease = dic.ContainsKey("近3年") ? _convertService.ConvertToNullableDecimal(dic["近3年"]) : null,
                        OneYearIncrease = dic.ContainsKey("近1年") ? _convertService.ConvertToNullableDecimal(dic["近1年"]) : null,
                        SixMonthIncrease = dic.ContainsKey("近6月") ? _convertService.ConvertToNullableDecimal(dic["近6月"]) : null,
                        ThreeMonthIncrease = dic.ContainsKey("近3月") ? _convertService.ConvertToNullableDecimal(dic["近3月"]) : null,
                        OneMonthIncrease = dic.ContainsKey("近1月") ? _convertService.ConvertToNullableDecimal(dic["近1月"]) : null,
                        Size = dic.ContainsKey("基金规模") ? _convertService.ConvertToDecimal(dic["基金规模"]) : null,
                        Manager = dic.ContainsKey("基金经理") ? dic["基金经理"] : null,
                        CreationDate = dic.ContainsKey("成 立 日") ? _convertService.ConvertToDate(dic["成 立 日"]) : null,
                        Active = true,
                        EventTime = DateTime.Now,
                        AuditBy = "",
                        EventType = "I"
                    };
                    list.Add(newItem);
                }
                catch (Exception ex)
                {

                }
            }
            var existing = (await _fundQuery.GetValueObjectsAsync()).Select(x=>x.Id).ToList();
            var toBeSaved = list.Where(x => !existing.Contains(x.Id)).ToList();
            if (toBeSaved.Any())
            {
                _logger.LogInformation($"Saved {toBeSaved.Count} funds into database!");
                await _fundWriter.AddRangeAsync(toBeSaved, true);
            }
        }
    }
}
