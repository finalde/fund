using System;
using Data.Contract;
using Domain.SharedKernel.ValueObjects;

namespace BusinessLayer.Query.Mappers
{
    public interface IMorningStarMapper
    {
        MorningStarValueObject Map(MorningStar item);
    }
    public class MorningStarMapper: IMorningStarMapper
    {
        public MorningStarValueObject Map(MorningStar item)
        {
            return new MorningStarValueObject(
                item.Id,
                item.Date,
                item.Code,
                item.Name,
                item.Type,
                item.ThreeYearRating,
                item.FiveYearRating,
                item.ValueDate,
                item.UnitValue,
                item.DailyChange,
                item.CurrentYearReturn,
                item.Active,
                item.AuditBy,
                item.EventType,
                item.EventTime,
                item.DbTime
                );
        }
    }
}
