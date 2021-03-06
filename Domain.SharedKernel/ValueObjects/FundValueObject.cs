using System;
using System.Collections.Generic;
using Common;

namespace Domain.SharedKernel.ValueObjects
{
    public class FundValueObject : ValueObject
    {
        public long Id { get;}
        public DateTime Date { get; set; }
        public string Code { get; }
        public string ShortName { get; }
        public string Name { get; }
        public string Type { get; }
        public string FullName { get; }
        public decimal? OneYearIncrease { get;}
        public decimal? SixMonthIncrease { get;}
        public decimal? ThreeMonthIncrease { get;}
        public decimal? OneMonthIncrease { get;}
        public decimal? Size { get;}
        public string Manager { get;}
        public DateTime? CreationDate { get;}
        public bool Active { get; }
        public string AuditBy { get; }
        public string EventType { get; }
        public DateTime EventTime { get; }
        public DateTime? DbTime { get; }

        public FundValueObject(long id, DateTime date, string code, string shortName, string name, string type, string fullName, decimal? oneYearIncrease, decimal? sixMonthIncrease, decimal? threeMonthIncrease, decimal? oneMonthIncrease, decimal? size, string manager, DateTime? creationDate, bool active, string auditBy, string eventType, DateTime eventTime, DateTime? dbTime)
        {
            Id = id;
            Date = date;
            Code = code;
            ShortName = shortName;
            Name = name;
            Type = type;
            FullName = fullName;
            OneYearIncrease = oneYearIncrease;
            SixMonthIncrease = sixMonthIncrease;
            ThreeMonthIncrease = threeMonthIncrease;
            OneMonthIncrease = oneMonthIncrease;
            Size = size;
            Manager = manager;
            CreationDate = creationDate;
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
            yield return ShortName;
            yield return Name;
            yield return Type;
            yield return FullName;
            yield return OneYearIncrease;
            yield return SixMonthIncrease;
            yield return ThreeMonthIncrease;
            yield return OneMonthIncrease;
            yield return Size;
            yield return Manager;
            yield return CreationDate;
            yield return Active;
            yield return AuditBy;
            yield return EventType;
            yield return EventTime;
            yield return DbTime;

        }
    }
}
