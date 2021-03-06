using System;
using Data.Contract;
using Domain.SharedKernel.ValueObjects;

namespace BusinessLayer.Query.Mappers
{
    public interface IIndexFundMapper
    {
        IndexFundValueObject Map(IndexFund item);
    }
    public class IndexFundMapper : IIndexFundMapper
    {
        public IndexFundValueObject Map(IndexFund item)
        {
            return new IndexFundValueObject(
                item.Id,
                item.Date,
                item.Code,
                item.Open,
                item.High,
                item.Low,
                item.Close,
                item.PreClose,
                item.Change,
                item.PctChg,
                item.Volume,
                item.Amount,
                item.Active,
                item.AuditBy,
                item.EventType,
                item.EventTime,
                item.DbTime
                );
        }
    }
}
