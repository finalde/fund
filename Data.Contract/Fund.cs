using System;
namespace Data.Contract
{
    public class Fund : BaseDateDao<long>
    {
        public string Code { get; set; }
        public string ShortName { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string FullName { get; set; }
        public decimal? SinceInceptionIncrease { get; set; }
        public decimal? ThreeYearIncrease { get; set; }
        public decimal? OneYearIncrease { get; set; }
        public decimal? SixMonthIncrease { get; set; }
        public decimal? ThreeMonthIncrease { get; set; }
        public decimal? OneMonthIncrease { get; set; }
        public decimal? Size { get; set; }
        public string Manager { get; set; }
        public DateTime? CreationDate { get; set; }
    }
}