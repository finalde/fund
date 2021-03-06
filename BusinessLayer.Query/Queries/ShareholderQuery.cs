using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessLayer.Query.Mappers;
using Data.Contract;
using Data.Reader;
using Domain.SharedKernel.ValueObjects;

namespace BusinessLayer.Query.Queries
{
    public interface IShareholderQuery
    {
        Task<IList<ShareholderValueObject>> GetValueObjectsAsync(DateTime date);
    }
    public class ShareholderQuery : IShareholderQuery
    {
        private readonly IDateReader<long, Shareholder> _shareholderReader;
        private readonly IShareholderMapper _shareholderMapper;

        public ShareholderQuery(IDateReader<long, Shareholder> shareholderReader, IShareholderMapper shareholderMapper)
        {
            _shareholderReader = shareholderReader;
            _shareholderMapper = shareholderMapper;
        }
        public async Task<IList<ShareholderValueObject>> GetValueObjectsAsync(DateTime date)
        {
            var data = await _shareholderReader.GetAsync(date);
            return data.Select(_shareholderMapper.Map).ToList();
        }
    }
}
