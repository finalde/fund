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
    public interface IFundQuery
    {
        Task<IList<FundValueObject>> GetValueObjectsAsync();
    }
    public class FundQuery: IFundQuery
    {
        private readonly IDateReader<long, Fund> _fundReader;
        private readonly IFundMapper _fundMapper;

        public FundQuery(IDateReader<long, Fund> fundReader, IFundMapper fundMapper)
        {
            _fundReader = fundReader ?? throw new ArgumentNullException(nameof(fundReader));
            _fundMapper = fundMapper ?? throw new ArgumentNullException(nameof(fundMapper));
        }
        public async Task<IList<FundValueObject>> GetValueObjectsAsync()
        {
            var data = await _fundReader.GetAllAsync();
            return data.Select(_fundMapper.Map).ToList();
        }
    }
}
