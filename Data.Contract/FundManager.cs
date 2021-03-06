using System;
namespace Data.Contract
{
    public class FundManager : BaseDateDao<long>
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string CompanyCode { get; set; }
        public string CompanyName { get; set; }
        public string FundCodes { get; set; }
        public string FundNames { get; set; }
        public int ExperienceInDays { get; set; }
        public string BestPerformFundCode { get; set; }
        public string BestPerformFundName { get; set; }
        public decimal TotalAssetUnderManagement { get; set; }
        public decimal BestFundReturn { get; set; }
    }
}
