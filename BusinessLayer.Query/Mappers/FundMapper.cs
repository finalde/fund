using System;
using Data.Contract;
using Domain.SharedKernel.ValueObjects;

namespace BusinessLayer.Query.Mappers
{
    public interface IFundMapper
    {
        FundValueObject Map(Fund item);
    }
    public class FundMapper: IFundMapper
    {
        public FundValueObject Map(Fund item)
        {
            return new FundValueObject(
                item.Id,
                item.Date,
                item.Code,
                item.ShortName,
                item.Name,
                item.Type,
                item.FullName,
                item.OneYearIncrease,
                item.SixMonthIncrease,
                item.ThreeMonthIncrease,
                item.OneMonthIncrease,
                item.Size,
                item.Manager,
                item.CreationDate,
                item.Active,
                item.AuditBy,
                item.EventType,
                item.EventTime,
                item.DbTime);
        }
    }
}
