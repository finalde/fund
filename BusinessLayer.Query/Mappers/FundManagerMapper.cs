using System;
using Data.Contract;
using Domain.SharedKernel.ValueObjects;

namespace BusinessLayer.Query.Mappers
{
    public interface IFundManagerMapper
    {
        FundManagerValueObject Map(FundManager item);
    }
    public class FundManagerMapper : IFundManagerMapper
    {
        public FundManagerValueObject Map(FundManager item)
        {
            return new FundManagerValueObject(
                item.Id,
                item.Date,
                item.Code,
                item.Name,
                item.CompanyCode,
                item.CompanyName,
                item.FundCodes,
                item.FundNames,
                item.ExperienceInDays,
                item.BestPerformFundCode,
                item.BestPerformFundName,
                item.TotalAssetUnderManagement,
                item.BestFundReturn,
                item.Active,
                item.AuditBy,
                item.EventType,
                item.EventTime,
                item.DbTime
                );
        }
    }
}
