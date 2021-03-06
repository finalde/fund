using System;
namespace Data.Contract
{
    public class IndexFund : BaseDateDao<long>
    {
        public string Code { get; set; }
        public decimal? Open { get; set; }
        public decimal? High { get; set; }
        public decimal? Low { get; set; }
        public decimal? Close { get; set; }
        public decimal? PreClose { get; set; }
        public decimal? Change { get; set; }
        public decimal? PctChg { get; set; }
        public decimal? Volume { get; set; }
        public decimal? Amount { get; set; }
    }
}
