using System;
using System.Collections.Generic;
using Common;

namespace Domain.SharedKernel.ValueObjects
{
    public class FundDetailValueObject : ValueObject
    {
        public long Id { get; }
        public DateTime Date { get; }
        public string Fund { get; }
        public decimal? UnitValue { get; }
        public decimal? CumulativeValue { get; }
        public decimal? DailyIncrease { get; }

        public decimal? PnlPerTenThousandsShare { get; }
        public decimal? AnualizedProfitSevenDays { get; }

        public string CreationStatus { get; }
        public string RedemptionStatus { get; }
        public string Dividend { get; }
        public bool Active { get; }
        public string AuditBy { get; }
        public string EventType { get; }
        public DateTime EventTime { get; }
        public DateTime? DbTime { get; }

        public FundDetailValueObject(long id, DateTime date, string fund, decimal? unitValue, decimal? cumulativeValue, decimal? dailyIncrease, decimal? pnlPerTenThousandsShare, decimal? anualizedProfitSevenDays, string creationStatus, string redemptionStatus, string dividend, bool active, string auditBy, string eventType, DateTime eventTime, DateTime? dbTime)
        {
            Id = id;
            Date = date;
            Fund = fund;
            UnitValue = unitValue;
            CumulativeValue = cumulativeValue;
            DailyIncrease = dailyIncrease;
            PnlPerTenThousandsShare = pnlPerTenThousandsShare;
            AnualizedProfitSevenDays = anualizedProfitSevenDays;
            CreationStatus = creationStatus;
            RedemptionStatus = redemptionStatus;
            Dividend = dividend;
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
            yield return Fund;
            yield return UnitValue;
            yield return CumulativeValue;
            yield return DailyIncrease;
            yield return PnlPerTenThousandsShare;
            yield return AnualizedProfitSevenDays;
            yield return CreationStatus;
            yield return RedemptionStatus;
            yield return Dividend;
            yield return Active;
            yield return AuditBy;
            yield return EventType;
            yield return EventTime;
            yield return DbTime;
        }
    }
}
