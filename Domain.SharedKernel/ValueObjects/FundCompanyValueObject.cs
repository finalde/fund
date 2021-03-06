using System;
using System.Collections.Generic;
using Common;

namespace Domain.SharedKernel.ValueObjects
{
    public class FundCompanyValueObject : ValueObject
    {
        public long Id { get; }
        public DateTime Date { get;}
        public string Code { get;}
        public string Name { get;}
        public DateTime CreationDate { get;}
        public int TotalFunds { get;}
        public string ManagingDirector { get;}
        public string Abbr { get;}
        public decimal AssetUnderManagement { get;}
        public int StarRating { get;}
        public string ShortName { get;}
        public DateTime StatisticsDate { get;}
        public bool Active { get; }
        public string AuditBy { get; }
        public string EventType { get; }
        public DateTime EventTime { get; }
        public DateTime? DbTime { get; }

        public FundCompanyValueObject(long id, DateTime date, string code, string name, DateTime creationDate, int totalFunds, string managingDirector, string abbr, decimal assetUnderManagement, int starRating, string shortName, DateTime statisticsDate, bool active, string auditBy, string eventType, DateTime eventTime, DateTime? dbTime)
        {
            Id = id;
            Date = date;
            Code = code;
            Name = name;
            CreationDate = creationDate;
            TotalFunds = totalFunds;
            ManagingDirector = managingDirector;
            Abbr = abbr;
            AssetUnderManagement = assetUnderManagement;
            StarRating = starRating;
            ShortName = shortName;
            StatisticsDate = statisticsDate;
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
            yield return CreationDate;
            yield return TotalFunds;
            yield return ManagingDirector;
            yield return Abbr;
            yield return AssetUnderManagement;
            yield return StarRating;
            yield return ShortName;
            yield return StatisticsDate;
            yield return Active;
            yield return AuditBy;
            yield return EventType;
            yield return EventTime;
            yield return DbTime;
        }
    }
}
