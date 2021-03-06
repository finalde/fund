using System;
using Data.Contract;
using Domain.SharedKernel.ValueObjects;

namespace BusinessLayer.Query.Mappers
{
    public interface IFundCompanyMapper
    {
        FundCompanyValueObject Map(FundCompany item);
    }
    public class FundCompanyMapper : IFundCompanyMapper
    {
        public FundCompanyValueObject Map(FundCompany item)
        {
            return new FundCompanyValueObject(
                item.Id,
                item.Date,
                item.Code,
                item.Name,
                item.CreationDate,
                item.TotalFunds,
                item.ManagingDirector,
                item.Abbr,
                item.AssetUnderManagement,
                item.StarRating,
                item.ShortName,
                item.StatisticsDate,
                item.Active,
                item.AuditBy,
                item.EventType,
                item.EventTime,
                item.DbTime
                );
        }
    }
}
