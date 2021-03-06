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
using Newtonsoft.Json.Linq;

namespace FundImporter.Bls
{
    public class JsonFundManager
    {
        public JArray data { get; set; }
        public int record { get; set; }
        public int pages { get; set; }
        public int curpage { get; set; }
    }
    public interface IFundManagerBl
    {
        Task RunAsync();
    }
    public class FundManagerBl : IFundManagerBl
    {
        private readonly IConvertService _convertService;
        private readonly IDateWriter<long, FundManager> _fundManagerWriter;
        private readonly IFundManagerQuery _fundManagerQuery;
        private readonly ILogger<FundManagerBl> _logger;

        public FundManagerBl(IConvertService convertService, IDateWriter<long, FundManager> fundManagerWriter, IFundManagerQuery fundManagerQuery, ILogger<FundManagerBl> logger)
        {
            _convertService = convertService ?? throw new ArgumentNullException(nameof(convertService));
            _fundManagerWriter = fundManagerWriter ?? throw new ArgumentNullException(nameof(fundManagerWriter));
            _fundManagerQuery = fundManagerQuery ?? throw new ArgumentNullException(nameof(fundManagerQuery));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task RunAsync()
        {
            var existing = await _fundManagerQuery.GetValueObjectsAsync(DateTime.Now.Date);
            if (existing.Any())
            {
                _logger.LogInformation($"FundManager {existing.Count} records eixsts already, skipping...");
                return;
            }
            using var client = new WebClient();
            var str = client.DownloadString(new Uri("http://fund.eastmoney.com/Data/FundDataPortfolio_Interface.aspx?dt=14&mc=returnjson&ft=all&pn=2160&pi=2&sc=abbname&st=asc"));
            str = str.Replace("var returnjson= ", "").Trim();
            var pageInfo = JsonConvert.DeserializeObject<JsonFundManager>(str);
            var list = new List<FundManager>();
            for (int page = 1; page <= pageInfo.pages; page++)
            {
                var pageUrl = $"http://fund.eastmoney.com/Data/FundDataPortfolio_Interface.aspx?dt=14&mc=returnjson&ft=all&pn=2160&pi={page}&sc=abbname&st=asc";
                var pageStr = client.DownloadString(new Uri(pageUrl));
                pageStr = pageStr.Replace("var returnjson= ", "").Trim().TrimEnd(';');
                var data = JsonConvert.DeserializeObject<JsonFundManager>(pageStr);
                foreach (var item in data.data)
                {
                    var props = item.Children().Select(x => x.ToString()).ToList();
                    try
                    {
                        var newItem = new FundManager
                        {
                            Id = 0,
                            Date = DateTime.Now.Date,
                            Code = props[0],
                            Name = props[1],
                            CompanyCode = props[2],
                            CompanyName = props[3],
                            FundCodes = props[4],
                            FundNames = props[5],
                            ExperienceInDays = _convertService.ConvertToInt(props[6]),
                            BestPerformFundCode = props[8],
                            BestPerformFundName = props[9],
                            TotalAssetUnderManagement = _convertService.ConvertToDecimal(props[10]),
                            BestFundReturn = _convertService.ConvertToDecimal(props[11]),
                            Active = true,
                            EventTime = DateTime.Now,
                            AuditBy = "",
                            EventType = "I"
                        };
                        list.Add(newItem);
                    }catch(Exception ex)
                    {

                    }
                }
            }
            if (list.Any())
            {
                await _fundManagerWriter.AddRangeAsync(list, true);
                _logger.LogInformation($"Saved {list.Count} fund manager records.");
            }
        }
    }
}
