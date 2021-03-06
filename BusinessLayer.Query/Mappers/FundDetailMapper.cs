using System;
using Data.Contract;
using Domain.SharedKernel.ValueObjects;

namespace BusinessLayer.Query.Mappers
{
    public interface IFundDetailMapper
    {
        FundDetailValueObject Map(FundDetail item);
    }
    public class FundDetailMapper : IFundDetailMapper
    {
        public FundDetailValueObject Map(FundDetail item)
        {
            return new FundDetailValueObject(
                item.Id,
                item.Date,
                item.Fund,
                item.UnitValue,
                item.CumulativeValue,
                item.DailyIncrease,
                item.PnlPerTenThousandsShare,
                item.AnualizedProfitSevenDays,
                item.CreationStatus,
                item.RedemptionStatus,
                item.Dividend,
                item.Active,
                item.AuditBy,
                item.EventType,
                item.EventTime,
                item.DbTime
                );
        }
    }
}
