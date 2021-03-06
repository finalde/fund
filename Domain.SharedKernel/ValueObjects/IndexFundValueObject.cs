using System;
using System.Collections.Generic;
using Common;

namespace Domain.SharedKernel.ValueObjects
{
    public class IndexFundValueObject : ValueObject
    {
        public long Id { get; }
        public DateTime Date { get; }
        public string Code { get; }
        public decimal? Open { get; }
        public decimal? High { get; }
        public decimal? Low { get; }
        public decimal? Close { get; }
        public decimal? PreClose { get; }
        public decimal? Change { get; }
        public decimal? PctChg { get; }
        public decimal? Volume { get; }
        public decimal? Amount { get; }
        public bool Active { get; }
        public string AuditBy { get; }
        public string EventType { get; }
        public DateTime EventTime { get; }
        public DateTime? DbTime { get; }

        public IndexFundValueObject(long id, DateTime date, string code, decimal? open, decimal? high, decimal? low, decimal? close, decimal? preClose, decimal? change, decimal? pctChg, decimal? volume, decimal? amount, bool active, string auditBy, string eventType, DateTime eventTime, DateTime? dbTime)
        {
            Id = id;
            Date = date;
            Code = code;
            Open = open;
            High = high;
            Low = low;
            Close = close;
            PreClose = preClose;
            Change = change;
            PctChg = pctChg;
            Volume = volume;
            Amount = amount;
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
            yield return Open;
            yield return High;
            yield return Low;
            yield return Close;
            yield return PreClose;
            yield return Change;
            yield return PctChg;
            yield return Volume;
            yield return Amount;
            yield return Active;
            yield return AuditBy;
            yield return EventType;
            yield return EventTime;
            yield return DbTime;
        }
    }
}
