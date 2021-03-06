using System;
namespace Data.Contract
{
    public class FundCalculatedData : BaseDateDao<long>
    {
        public string Fund { get; set; }
        public decimal? BetaInception { get; set; }
        public decimal? AlphaInception { get; set; }
        public DateTime? InceptionDate { get; set; }
        public decimal? TreynorInception { get; set; }

        public decimal? BetaCurrent { get; set; }
        public decimal? AlphaCurrent { get; set; }
        public decimal? TreynorCurrent{ get; set; }
    }
}
