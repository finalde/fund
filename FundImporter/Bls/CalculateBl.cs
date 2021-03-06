using System;
using System.Linq;
using System.Threading.Tasks;
using BusinessLayer.Query.Queries;
using MathNet.Numerics.Statistics;
using Common.Extensions;
using Data.Contract;
using System.Collections.Generic;
using Data.Writer;
using Microsoft.Extensions.Logging;

namespace FundImporter.Bls
{
    public interface ICalculateBl
    {
        Task RunAsync();
    }
    public class CalculateBl : ICalculateBl
    {
        private readonly IFundDetailQuery _fundDetailQuery;
        private readonly IIndexFundQuery _indexFundQuery;
        private readonly IDateWriter<long, FundCalculatedData> _fundCalcualtedDataWriter;
        private readonly ILogger<CalculateBl> _logger;
        private const decimal RiskFreeRate = 0.025m;
        public CalculateBl(IFundDetailQuery fundDetailQuery, IIndexFundQuery indexFundQuery, IDateWriter<long, FundCalculatedData> fundCalcualtedDataWriter, ILogger<CalculateBl> logger)
        {
            _fundDetailQuery = fundDetailQuery;
            _indexFundQuery = indexFundQuery;
            _fundCalcualtedDataWriter = fundCalcualtedDataWriter;
            _logger = logger;
        }
        private decimal? Multiply(IList<decimal> list)
        {
            decimal result = 1;
            var index = 0;
            foreach(var item in list)
            {

                result *= item;
                index++;
                if(index % 200 == 0)
                {

                }
            }
            decimal? data = (result - 1);
            data = data > 10 || data < -10 ? null : decimal.Round(data.GetValueOrDefault(), 6);
            return data;
        }
        public async Task RunAsync()
        {
            var data = (await _fundDetailQuery.GetAllValueObjectAsync())
                .Where(x=>x.DailyIncrease is not null)
                //.Where(x=>x.Fund == "570008")
                .GroupBy(x => x.Fund)
                .Where(x=>x.Count() > 100)
                .ToDictionary(x => x.Key, v => v.OrderBy(x => x.Date).ToList());
            var csi300 = (await _indexFundQuery.GetAllValueObjectsAsync())
                .Where(x => x.Code == "000300.SH")
                .OrderBy(x => x.Date)
                .ToList();
            var csi300Dates = csi300.Select(x=>x.Date).OrderBy(x => x).ToList();
            var list = new List<FundCalculatedData>();
            foreach (var item in data)
            {
                var fundDateInception = item.Value.Select(x => x.Date).Distinct().ToList();
                var fundDateCurrent = item.Value.Select(x => x.Date).Where(x=>x >= DateTime.Now.Date.AddYears(-1)).Distinct().ToList();

                var keysInception = csi300Dates.Intersect(fundDateInception).OrderBy(x => x).ToList();
                var keysCurrent = csi300Dates.Intersect(fundDateCurrent).OrderBy(x => x).ToList();

                var fundsInception = item.Value.Where(x => keysInception.Contains(x.Date)).Select(x=>(double)x.DailyIncrease.GetValueOrDefault()).ToList();
                var fundsCurrent = item.Value.Where(x => keysCurrent.Contains(x.Date)).Select(x => (double)x.DailyIncrease.GetValueOrDefault()).ToList();

                var csi300sInception = csi300.Where(x => keysInception.Contains(x.Date)).Select(x => (double)x.PctChg.GetValueOrDefault()).ToList();
                var csi300sCurrent= csi300.Where(x => keysCurrent.Contains(x.Date)).Select(x => (double)x.PctChg.GetValueOrDefault()).ToList();

                var varianceInception = Statistics.Variance(fundsInception.Select(x=>1+x));
                var covarianceInception = Statistics.Covariance(fundsInception.Select(x => 1 + x), csi300sInception.Select(x => 1 + x));

                var varianceCurrent = Statistics.Variance(fundsCurrent.Select(x => 1 + x));
                var covarianceCurrent = Statistics.Covariance(fundsCurrent.Select(x => 1 + x), csi300sCurrent.Select(x => 1 + x));
                if (double.IsNaN(varianceInception) || double.IsNaN(covarianceInception) || double.IsNaN(varianceCurrent) || double.IsNaN(covarianceCurrent))
                {
                    continue;
                }
                try
                {
                    var stockExptectedReturnInception = Multiply(fundsInception.Select(x=>(decimal)x+ 1).ToList());
                    var marketExpectedReturnInception = Multiply(csi300sInception.Select(x => (decimal)x + 1).ToList());

                    var betaInception = ((decimal)covarianceInception).SafeDivideBy((decimal)varianceInception);
                    var alphaInception = stockExptectedReturnInception - (RiskFreeRate + betaInception * (marketExpectedReturnInception - RiskFreeRate));

                    var treynorInception = (stockExptectedReturnInception.GetValueOrDefault() - RiskFreeRate).SafeDivideBy(betaInception);

                    var stockExptectedReturnCurrent = Multiply(fundsCurrent.Select(x => (decimal)x + 1).ToList());
                    var marketExpectedReturnCurrent = Multiply(csi300sCurrent.Select(x => (decimal)x + 1).ToList());

                    var betaCurrent = ((decimal)covarianceCurrent).SafeDivideBy((decimal)varianceCurrent);
                    var alphaCurrent = stockExptectedReturnCurrent - (RiskFreeRate + betaCurrent * (marketExpectedReturnCurrent - RiskFreeRate));
                    var treynorCurrent = (stockExptectedReturnCurrent.GetValueOrDefault() - RiskFreeRate).SafeDivideBy(betaCurrent);

                    var newItem = new FundCalculatedData
                    {
                        Id = 0,
                        Date = DateTime.Now.Date,
                        Fund = item.Key,
                        InceptionDate = keysInception.OrderBy(x=>x).FirstOrDefault(),
                        BetaInception = betaInception,
                        AlphaInception = alphaInception,
                        TreynorInception = treynorInception,
                        BetaCurrent = betaCurrent,
                        AlphaCurrent = alphaCurrent,
                        TreynorCurrent = treynorCurrent,
                        Active = true,
                        EventTime = DateTime.Now,
                        AuditBy = "",
                        EventType = "I"
                    };
                    list.Add(newItem);
                }
                catch(Exception ex)
                {

                }
            }
            if (list.Any())
            {
                await _fundCalcualtedDataWriter.AddRangeAsync(list, true);
                _logger.LogInformation($"FundCalculatedData saved {list.Count} records");
            }
        }
    }
}
