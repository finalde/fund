using System;
using System.Collections.Generic;
using Common;

namespace Domain.SharedKernel.ValueObjects
{
    public class ShareholderValueObject : ValueObject
    {
        public long Id { get; }
        public DateTime Date { get; }
        public string Fund { get; }
        public decimal? Institution { get; }
        public decimal? Individual { get; }
        public decimal? Internal { get; }
        public decimal? TotalAmount { get; }
        public bool Active { get; }
        public string AuditBy { get; }
        public string EventType { get; }
        public DateTime EventTime { get; }
        public DateTime? DbTime { get; }

        public ShareholderValueObject(long id, DateTime date, string fund, decimal? institution, decimal? individual, decimal? @internal, decimal? totalAmount, bool active, string auditBy, string eventType, DateTime eventTime, DateTime? dbTime)
        {
            Id = id;
            Date = date;
            Fund = fund;
            Institution = institution;
            Individual = individual;
            Internal = @internal;
            TotalAmount = totalAmount;
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
            yield return Institution;
            yield return Individual;
            yield return Internal;
            yield return TotalAmount;
            yield return Active;
            yield return AuditBy;
            yield return EventType;
            yield return EventTime;
            yield return DbTime;
        }
    }
}
