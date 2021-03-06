using System;
namespace Data.Contract
{
    public class Shareholder : BaseDateDao<long>
    {
        public string Fund { get; set; }
        public decimal? Institution { get; set; }
        public decimal? Individual { get; set; }
        public decimal? Internal { get; set; }
        public decimal? TotalAmount { get; set; }
    }
}
