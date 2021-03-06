using System;
namespace Data.Contract
{
    public class MorningStar:BaseDateDao<long>
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public int? ThreeYearRating { get; set; }
        public int? FiveYearRating { get; set; }
        public DateTime? ValueDate { get; set; }
        public decimal? UnitValue { get; set; }
        public decimal? DailyChange { get; set; }
        public decimal? CurrentYearReturn { get; set; }
    }
}
