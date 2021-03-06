using System;
using System.Collections.Generic;
using Common;

namespace Domain.SharedKernel.ValueObjects
{
    public class MorningStarValueObject : ValueObject
    {
        public long Id { get; }
        public DateTime Date { get; }
        public string Code { get; }
        public string Name { get; }
        public string Type { get; }
        public int? ThreeYearRating { get; }
        public int? FiveYearRating { get; }
        public DateTime? ValueDate { get; }
        public decimal? UnitValue { get; }
        public decimal? DailyChange { get; }
        public decimal? CurrentYearReturn { get; }
        public bool Active { get; }
        public string AuditBy { get; }
        public string EventType { get; }
        public DateTime EventTime { get; }
        public DateTime? DbTime { get; }

        public MorningStarValueObject(long id, DateTime date, string code, string name, string type, int? threeYearRating, int? fiveYearRating, DateTime? valueDate, decimal? unitValue, decimal? dailyChange, decimal? currentYearReturn, bool active, string auditBy, string eventType, DateTime eventTime, DateTime? dbTime)
        {
            Id = id;
            Date = date;
            Code = code;
            Name = name;
            Type = type;
            ThreeYearRating = threeYearRating;
            FiveYearRating = fiveYearRating;
            ValueDate = valueDate;
            UnitValue = unitValue;
            DailyChange = dailyChange;
            CurrentYearReturn = currentYearReturn;
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
            yield return Type;
            yield return ThreeYearRating;
            yield return FiveYearRating;
            yield return ValueDate;
            yield return UnitValue;
            yield return DailyChange;
            yield return CurrentYearReturn;
            yield return Active;
            yield return AuditBy;
            yield return EventType;
            yield return EventTime;
            yield return DbTime;
        }
    }
}
