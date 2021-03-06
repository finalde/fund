using System;
namespace Data.Contract
{
    public class FundDetail : BaseDateDao<long>
    {
        public string Fund { get; set; }
        public decimal? UnitValue { get; set; }
        public decimal? CumulativeValue { get; set; }
        public decimal? DailyIncrease { get; set; }

        public decimal? PnlPerTenThousandsShare { get; set; }
        public decimal? AnualizedProfitSevenDays { get; set; }

        public string CreationStatus { get; set; }
        public string RedemptionStatus { get; set; }
        public string Dividend { get; set; }
    }
}
