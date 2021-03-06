using System;
using Data.Contract;
using Domain.SharedKernel.ValueObjects;

namespace BusinessLayer.Query.Mappers
{
    public interface IShareholderMapper
    {
        ShareholderValueObject Map(Shareholder item);
    }
    public class ShareholderMapper : IShareholderMapper
    {
        public ShareholderValueObject Map(Shareholder item)
        {
            return new ShareholderValueObject(
                item.Id,
                item.Date,
                item.Fund,
                item.Institution,
                item.Individual,
                item.Internal,
                item.TotalAmount,
                item.Active,
                item.AuditBy,
                item.EventType,
                item.EventTime,
                item.DbTime
                );
        }
    }
}
