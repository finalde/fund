using System;
namespace Data.Contract
{
    public class FundCompany : BaseDateDao<long>
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public DateTime CreationDate { get; set; }
        public int TotalFunds { get; set; }
        public string ManagingDirector { get; set; }
        public string Abbr { get; set; }
        public decimal AssetUnderManagement { get; set; }
        public int StarRating { get; set; }
        public string ShortName { get; set; }
        public DateTime StatisticsDate { get; set; }
    }
}
