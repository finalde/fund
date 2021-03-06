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
    public interface IFundManagerQuery
    {
        Task<IList<FundManagerValueObject>> GetValueObjectsAsync(DateTime date);
    }
    public class FundManagerQuery : IFundManagerQuery
    {
        private readonly IDateReader<long, FundManager> _fundManagerReader;
        private readonly IFundManagerMapper _fundManagerMapper;

        public FundManagerQuery(IDateReader<long, FundManager> fundManagerReader, IFundManagerMapper fundManagerMapper)
        {
            _fundManagerReader = fundManagerReader;
            _fundManagerMapper = fundManagerMapper;
        }
        public async Task<IList<FundManagerValueObject>> GetValueObjectsAsync(DateTime date)
        {
            var data = await _fundManagerReader.GetAsync(date);
            return data.Select(_fundManagerMapper.Map).ToList();
        }
    }
}
