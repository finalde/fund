using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BusinessLayer.Query.Queries;
using Data.Contract;
using Data.Writer;
using FundImporter.Services;
using Microsoft.Extensions.Logging;

namespace FundImporter.Bls
{
    public interface IMorningstarBl
    {
        Task RunAsync();
    }
    public class MorningstarBl : IMorningstarBl
    {
        private readonly IConvertService _convertService;
        private readonly IDateWriter<long, MorningStar> _morningStarWriter;
        private readonly IMorningStarQuery _morningStarQuery;
        private readonly ILogger<MorningstarBl> _logger;

        public MorningstarBl(IConvertService convertService, IDateWriter<long, MorningStar> morningStarWriter, IMorningStarQuery morningStarQuery, ILogger<MorningstarBl> logger)
        {
            _convertService = convertService;
            _morningStarWriter = morningStarWriter;
            _morningStarQuery = morningStarQuery;
            _logger = logger;
        }

        private readonly string ThreeStar = $"<img src={"\""}data:image/gif;base64,R0lGODlhQgARAPcAAP//////zP//mf//Zv//M///AP/M///MzP/Mmf/MZv/MM//MAP+Z//+ZzP+Zmf+ZZv+ZM/+ZAP9m//9mzP9mmf9mZv9mM/9mAP8z//8zzP8zmf8zZv8zM/8zAP8A//8AzP8Amf8AZv8AM/8AAMz//8z/zMz/mcz/Zsz/M8z/AMzM/8zMzMzMmczMZszMM8zMAMyZ/8yZzMyZmcyZZsyZM8yZAMxm/8xmzMxmmcxmZsxmM8xmAMwz/8wzzMwzmcwzZswzM8wzAMwA/8wAzMwAmcwAZswAM8wAAJn//5n/zJn/mZn/Zpn/M5n/AJnM/5nMzJnMmZnMZpnMM5nMAJmZ/5mZzJmZmZmZZpmZM5mZAJlm/5lmzJlmmZlmZplmM5lmAJkz/5kzzJkzmZkzZpkzM5kzAJkA/5kAzJkAmZkAZpkAM5kAAGb//2b/zGb/mWb/Zmb/M2b/AGbM/2bMzGbMmWbMZmbMM2bMAGaZ/2aZzGaZmWaZZmaZM2aZAGZm/2ZmzGZmmWZmZmZmM2ZmAGYz/2YzzGYzmWYzZmYzM2YzAGYA/2YAzGYAmWYAZmYAM2YAADP//zP/zDP/mTP/ZjP/MzP/ADPM/zPMzDPMmTPMZjPMMzPMADOZ/zOZzDOZmTOZZjOZMzOZADNm/zNmzDNmmTNmZjNmMzNmADMz/zMzzDMzmTMzZjMzMzMzADMA/zMAzDMAmTMAZjMAMzMAAAD//wD/zAD/mQD/ZgD/MwD/AADM/wDMzADMmQDMZgDMMwDMAACZ/wCZzACZmQCZZgCZMwCZAABm/wBmzABmmQBmZgBmMwBmAAAz/wAzzAAzmQAzZgAzMwAzAAAA/wAAzAAAmQAAZgAAMwAAAPf39+/v7+7u7ufn59/f393d3dfX18/Pz8fHx8DAwLu7u7i4uLCwsKqqqqCgoIiIiHd3d1VVVREREf///wAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAACH5BAEAAOsALAAAAABCABEAAAj/ANcJHEiwoMGDCBMqXMiwocOHECNKnOjwHEOLCzEqBMeQ48Ju17QpBCkyIUmF2cxhQ6ky4zUrCs+9jDkzIbht3zbiXMjqWjqFPX8mDLpxnUeEHI8O1BbomtOnTwNpYwq1qlSqVaNqwzaOHLib67h9JTcOG1evYMWCI7tSoBV1VdXBHPg27ly3cKHKJejNW8FvOQf2/RuY4Aq9KwwefqouccHFThsb3HYUbEHKAy0/rtpNMWfPUDtf9uv3W7bJpNeZPojuGqtyPTUObP06tkHasK/JFggOWzhv4LIp5e0buPCDrNAJ1IaOlcHky5s/V76OufOC4zxiW2swu8Dt5AxqJhNXsFzJ5eTNrjRPcHz58+uwcSvore33+XztU9zPv7///wAGqFBAADs={"\""} style={"\""}border-width:0px;{"\""}>";
        private readonly string FourStar = $"<img src={"\""}data:image/gif;base64,R0lGODlhQgARAPcAAP//////zP//mf//Zv//M///AP/M///MzP/Mmf/MZv/MM//MAP+Z//+ZzP+Zmf+ZZv+ZM/+ZAP9m//9mzP9mmf9mZv9mM/9mAP8z//8zzP8zmf8zZv8zM/8zAP8A//8AzP8Amf8AZv8AM/8AAMz//8z/zMz/mcz/Zsz/M8z/AMzM/8zMzMzMmczMZszMM8zMAMyZ/8yZzMyZmcyZZsyZM8yZAMxm/8xmzMxmmcxmZsxmM8xmAMwz/8wzzMwzmcwzZswzM8wzAMwA/8wAzMwAmcwAZswAM8wAAJn//5n/zJn/mZn/Zpn/M5n/AJnM/5nMzJnMmZnMZpnMM5nMAJmZ/5mZzJmZmZmZZpmZM5mZAJlm/5lmzJlmmZlmZplmM5lmAJkz/5kzzJkzmZkzZpkzM5kzAJkA/5kAzJkAmZkAZpkAM5kAAGb//2b/zGb/mWb/Zmb/M2b/AGbM/2bMzGbMmWbMZmbMM2bMAGaZ/2aZzGaZmWaZZmaZM2aZAGZm/2ZmzGZmmWZmZmZmM2ZmAGYz/2YzzGYzmWYzZmYzM2YzAGYA/2YAzGYAmWYAZmYAM2YAADP//zP/zDP/mTP/ZjP/MzP/ADPM/zPMzDPMmTPMZjPMMzPMADOZ/zOZzDOZmTOZZjOZMzOZADNm/zNmzDNmmTNmZjNmMzNmADMz/zMzzDMzmTMzZjMzMzMzADMA/zMAzDMAmTMAZjMAMzMAAAD//wD/zAD/mQD/ZgD/MwD/AADM/wDMzADMmQDMZgDMMwDMAACZ/wCZzACZmQCZZgCZMwCZAABm/wBmzABmmQBmZgBmMwBmAAAz/wAzzAAzmQAzZgAzMwAzAAAA/wAAzAAAmQAAZgAAMwAAAPf39+/v7+7u7ufn59/f393d3dfX18/Pz8fHx8DAwLu7u7i4uLCwsKqqqqCgoIiIiHd3d1VVVREREf///wAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAACH5BAEAAOsALAAAAABCABEAAAj/ALEJHEiwoMGDCBMqXMiQ4LqHECNKnEixosWLGDNq3Mjx4bmMHzGGvDjSIriM3a5pu5hypcWWLFVezGYOm8hrVkji1JnT4rmdJrd9w8jqWrqLRY9aTIrU6MWTJyVqC3StqlWrgbRNvco161auWLVSBVs1K7Zx5MCB27aOm1py42w+tKKOq7qec+tevRuRrl286/zuBezNm8RvQyWu2Lti4mKr6horZuyYssRtUdetpfjYarfKVz9PDg3a88Rthg1/y0YR3TVW5YqWfOgatuyJtWNfm70u922J4LCF8wYuW+aIrNA91IaO1cTky5s/V76OuXOJ0KtLlzguKjZw5CZqJhMnsZzL5eQjmo84vvz56ukhroeIjZtEb3I76t/Pv7///wAGyF9AADs={"\""} style={"\""}border-width:0px;{"\""}>";
        private readonly string FiveStar = $"<img src={"\""}data:image/gif;base64,R0lGODlhQgARAPcAAP//////zP//mf//Zv//M///AP/M///MzP/Mmf/MZv/MM//MAP+Z//+ZzP+Zmf+ZZv+ZM/+ZAP9m//9mzP9mmf9mZv9mM/9mAP8z//8zzP8zmf8zZv8zM/8zAP8A//8AzP8Amf8AZv8AM/8AAMz//8z/zMz/mcz/Zsz/M8z/AMzM/8zMzMzMmczMZszMM8zMAMyZ/8yZzMyZmcyZZsyZM8yZAMxm/8xmzMxmmcxmZsxmM8xmAMwz/8wzzMwzmcwzZswzM8wzAMwA/8wAzMwAmcwAZswAM8wAAJn//5n/zJn/mZn/Zpn/M5n/AJnM/5nMzJnMmZnMZpnMM5nMAJmZ/5mZzJmZmZmZZpmZM5mZAJlm/5lmzJlmmZlmZplmM5lmAJkz/5kzzJkzmZkzZpkzM5kzAJkA/5kAzJkAmZkAZpkAM5kAAGb//2b/zGb/mWb/Zmb/M2b/AGbM/2bMzGbMmWbMZmbMM2bMAGaZ/2aZzGaZmWaZZmaZM2aZAGZm/2ZmzGZmmWZmZmZmM2ZmAGYz/2YzzGYzmWYzZmYzM2YzAGYA/2YAzGYAmWYAZmYAM2YAADP//zP/zDP/mTP/ZjP/MzP/ADPM/zPMzDPMmTPMZjPMMzPMADOZ/zOZzDOZmTOZZjOZMzOZADNm/zNmzDNmmTNmZjNmMzNmADMz/zMzzDMzmTMzZjMzMzMzADMA/zMAzDMAmTMAZjMAMzMAAAD//wD/zAD/mQD/ZgD/MwD/AADM/wDMzADMmQDMZgDMMwDMAACZ/wCZzACZmQCZZgCZMwCZAABm/wBmzABmmQBmZgBmMwBmAAAz/wAzzAAzmQAzZgAzMwAzAAAA/wAAzAAAmQAAZgAAMwAAAPn5+ff39+7u7t3d3bu7u6qqqoiIiHd3d1VVVREREf///wAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAACH5BAEAAOIALAAAAABCABEAAAj/ALEJHEiwoMGDCBMqXMiwocOHECNKnJhQnEWL3i4avJjxokePHT9+DCmSo0iBHrdd02ZxoziVLEtahClz5sqaL296RMnxmpWWBTH6xOltaM2iP48a1YjNI6tr4HA+jVpzqlSoV6lq1BbomtevXwNp4wq2rFiyZcOO7ZrW61m2bcWitBKubLikFunaxStOL9i7Hv1+BXxRsFfCPFf8XSFS8WDGHx0fhuxR8rVwlC9axszUouVr2xqXDR15tGiwpCubBmrx2zVW3Z6SbP069rXZ4lzDli1St23cvnmzFsfqm0Vt31iJLH48+XLj4pAr/8g8unPq0KV31sbtY7eYx7tnJhsv7rtH7t7BR+/u0fxF9O1j8sS5k6JC+ieb4mdqH+H++v0FSFFAADs={"\""} style={"\""}border-width:0px;{"\""}>";
        public async Task RunAsync()
        {
            var files = Directory.GetFiles("/Users/dalu/Desktop/workspace/Fund/Files/Morningstar/20210220");

            HtmlAgilityPack.HtmlDocument htmlDoc = new HtmlAgilityPack.HtmlDocument();
            var list = new List<MorningStar>();
            foreach (var file in files)
            {
                var content = File.ReadAllText(file);
                htmlDoc.LoadHtml(content);
                var trs = htmlDoc.DocumentNode.SelectNodes("//tr")
                    .Where(x => x.ChildNodes.Count == 13).Skip(1).ToList();
                foreach (var tr in trs)
                {
                    var texts = tr.ChildNodes.Select(x => x.InnerText).ToList();
                    var threeYearRating = getRating(tr.ChildNodes[6].InnerHtml);
                    var fiveYearRating = getRating(tr.ChildNodes[7].InnerHtml);
                    var item = new MorningStar
                    {
                        Id = 0,
                        Date = DateTime.Now.Date,
                        Code = texts[3],
                        Name = texts[4],
                        Type = texts[5],
                        ThreeYearRating = threeYearRating,
                        FiveYearRating = fiveYearRating,
                        ValueDate = _convertService.ConvertToDate(texts[8]),
                        UnitValue = _convertService.ConvertToNullableDecimal(texts[9]),
                        DailyChange = _convertService.ConvertToNullableDecimal(texts[10]),
                        CurrentYearReturn = _convertService.ConvertToNullableDecimal(texts[11]),
                        Active = true,
                        EventTime = DateTime.Now,
                        AuditBy = "",
                        EventType = "I"
                    };
                    list.Add(item);
                }
            }
            await _morningStarWriter.AddRangeAsync(list, true);
        }
        private int getRating(string innerHtml)
        {
            if (innerHtml == FiveStar)
            {
                return 5;
            }
            else if (innerHtml == FourStar)
            {
                return 4;
            }
            else if (innerHtml == ThreeStar)
            {
                return 3;
            }
            else
            {
                return 0;
            }
        }
    }
}
