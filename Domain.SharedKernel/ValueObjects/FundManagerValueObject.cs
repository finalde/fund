using System;
using System.Collections.Generic;
using Common;

namespace Domain.SharedKernel.ValueObjects
{
    public class FundManagerValueObject : ValueObject
    {
        public long Id { get; }
        public DateTime Date { get; }
        public string Code { get; }
        public string Name { get; }
        public string CompanyCode { get; }
        public string CompanyName { get; }
        public string FundCodes { get; }
        public string FundNames { get; }
        public int ExperienceInDays { get; }
        public string BestPerformFundCode { get; }
        public string BestPerformFundName { get; }
        public decimal TotalAssetUnderManagement { get; }
        public decimal BestFundReturn { get; }
        public bool Active { get; }
        public string AuditBy { get; }
        public string EventType { get; }
        public DateTime EventTime { get; }
        public DateTime? DbTime { get; }

        public FundManagerValueObject(long id, DateTime date, string code, string name, string companyCode, string companyName, string fundCodes, string fundNames, int experienceInDays, string bestPerformFundCode, string bestPerformFundName, decimal totalAssetUnderManagement, decimal bestFundReturn, bool active, string auditBy, string eventType, DateTime eventTime, DateTime? dbTime)
        {
            Id = id;
            Date = date;
            Code = code;
            Name = name;
            CompanyCode = companyCode;
            CompanyName = companyName;
            FundCodes = fundCodes;
            FundNames = fundNames;
            ExperienceInDays = experienceInDays;
            BestPerformFundCode = bestPerformFundCode;
            BestPerformFundName = bestPerformFundName;
            TotalAssetUnderManagement = totalAssetUnderManagement;
            BestFundReturn = bestFundReturn;
            Active = active;
            AuditBy = auditBy;
            EventType = eventType;
            EventTime = eventTime;
            DbTime = dbTime;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Id;
            yield return Date;
            yield return Code;
            yield return Name;
            yield return CompanyCode;
            yield return CompanyName;
            yield return FundCodes;
            yield return FundNames;
            yield return ExperienceInDays;
            yield return BestPerformFundCode;
            yield return BestPerformFundName;
            yield return TotalAssetUnderManagement;
            yield return BestFundReturn;
            yield return Active;
            yield return AuditBy;
            yield return EventType;
            yield return EventTime;
            yield return DbTime;
        }
    }
}
