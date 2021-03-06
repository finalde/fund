using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using BusinessLayer.Query.Queries;
using Client.Tushare;
using Data.Contract;
using Data.Writer;
using FundImporter.Services;
using Microsoft.Extensions.Logging;

namespace FundImporter.Bls
{
    public interface ICsi300Bl
    {
        Task RunAsync();
    }
    public class Csi300Bl: ICsi300Bl
    {
        private readonly ITushareClient _tushareClient;
        private readonly IIndexFundQuery _indexFundQuery;
        private readonly ILogger<Csi300Bl> _logger;
        private readonly IConvertService _convertService;
        private readonly IDateWriter<long, IndexFund> _indexFundWriter;

        public Csi300Bl(ITushareClient tushareClient, IIndexFundQuery indexFundQuery, ILogger<Csi300Bl> logger, IConvertService convertService, IDateWriter<long, IndexFund> indexFundWriter)
        {
            _tushareClient = tushareClient ?? throw new ArgumentNullException(nameof(tushareClient));
            _indexFundQuery = indexFundQuery ?? throw new ArgumentNullException(nameof(indexFundQuery));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _convertService = convertService ?? throw new ArgumentNullException(nameof(convertService));
            _indexFundWriter = indexFundWriter ?? throw new ArgumentNullException(nameof(indexFundWriter));
        }

        public async Task RunAsync()
        {
            var existing = (await _indexFundQuery.GetAllValueObjectsAsync()).Where(x => x.Code == "000300.SH").OrderByDescending(x => x.Date).ToList();
            var endDate = DateTime.Now.Date;
            var startDate = existing.Any() ? existing.First().Date : new DateTime(2000, 1, 1);
            if(startDate >= endDate)
            {
                _logger.LogInformation($"Csi 300 already update to date.");
                return;
            }
            var data = await _tushareClient.GetCsi300Async(startDate, endDate);
            if (data.Data.Items.Any())
            {
                var list = new List<IndexFund>();
                foreach(var item in data.Data.Items)
                {
                    var date = _convertService.ConvertToDate(item[1]);
                    if (date <= startDate) continue;
                    var newItem = new IndexFund {
                        Id = 0,
                        Code = item[0],
                        Date = date,
                        Close = _convertService.ConvertToNullableDecimal(item[2]),
                        Open = _convertService.ConvertToNullableDecimal(item[3]),
                        High = _convertService.ConvertToNullableDecimal(item[4]),
                        Low = _convertService.ConvertToNullableDecimal(item[5]),
                        PreClose= _convertService.ConvertToNullableDecimal(item[6]),
                        Change= _convertService.ConvertToNullableDecimal(item[7]),
                        PctChg= _convertService.ConvertToNullableDecimal(item[8])/100,
                        Volume= _convertService.ConvertToNullableDecimal(item[9]),
                        Amount= _convertService.ConvertToNullableDecimal(item[10]),
                        Active = true,
                        EventTime = DateTime.Now,
                        AuditBy = "",
                        EventType = "I"
                    };
                    list.Add(newItem);
                }
                await _indexFundWriter.AddRangeAsync(list, true);
                _logger.LogInformation($"IndexFund saved {list.Count} csi300 records.");
            }
        }
    }
}
