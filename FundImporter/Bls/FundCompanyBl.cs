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
    public interface IFundCompanyBl
    {
        Task RunAsync();
    }
    public class FundCompanyBl: IFundCompanyBl
    {
        private readonly IConvertService _convertService;
        private readonly IDateWriter<long, FundCompany> _fundCompanyWriter;
        private readonly ILogger<FundCompanyBl> _logger;
        private readonly IFundCompanyQuery _fundCompanyQuery;

        public FundCompanyBl(IConvertService convertService, IDateWriter<long, FundCompany> fundCompanyWriter, ILogger<FundCompanyBl> logger, IFundCompanyQuery fundCompanyQuery)
        {
            _convertService = convertService ?? throw new ArgumentNullException(nameof(convertService));
            _fundCompanyWriter = fundCompanyWriter ?? throw new ArgumentNullException(nameof(fundCompanyWriter));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _fundCompanyQuery = fundCompanyQuery ?? throw new ArgumentNullException(nameof(fundCompanyQuery));
        }

        public async Task RunAsync()
        {
            var existing = await _fundCompanyQuery.GetValueObjectsAsync(DateTime.Now.Date);
            if (existing.Any())
            {
                _logger.LogInformation($"FundCompany {existing.Count} records eixsts already, skipping...");
                return;
            }
            using var client = new WebClient();
            var data = client.DownloadString(new Uri("http://fund.eastmoney.com/Data/FundRankScale.aspx?_=1613299110703"));
            data = data.Replace("var json={datas:", "").Trim().TrimEnd('}');
            var jarray = JsonConvert.DeserializeObject<JArray>(data);
            var list = new List<FundCompany>();
            foreach (var item in jarray)
            {
                var props = item.Children().Select(x => x.ToString()).ToList();
                var newItem = new FundCompany
                {
                    Id = 0,
                    Date = DateTime.Now.Date,
                    Code = props[0],
                    Name = props[1],
                    CreationDate = _convertService.ConvertToDate(props[2]),
                    TotalFunds = _convertService.ConvertToInt(props[3]),
                    ManagingDirector = props[4],
                    Abbr = props[5],
                    AssetUnderManagement = _convertService.ConvertToDecimal(props[7]),
                    StarRating = props[8].Length,
                    ShortName = props[9],
                    StatisticsDate = _convertService.ConvertToDate(props[11]),
                    Active = true,
                    EventTime = DateTime.Now,
                    AuditBy = "",
                    EventType = "I"
                };
                list.Add(newItem);
            }
            if (list.Any())
            {
                await _fundCompanyWriter.AddRangeAsync(list, true);
                _logger.LogInformation($"Saved {list.Count} fund company records.");
            }
            //'80000080','山西证券股份有限公司','1988-07-28','14','王怡里','SXZQ','','88.70','★★★','山西证券','','2020/12/31 0:00:00'
        }
    }
}
