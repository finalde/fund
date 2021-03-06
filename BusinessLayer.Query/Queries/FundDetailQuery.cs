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
    public interface IFundDetailQuery
    {
        Task<IList<FundDetailValueObject>> GetValueObjectsAsync(DateTime date);
        Task<IList<FundDetailValueObject>> GetAllValueObjectAsync();
    }
    public class FundDetailQuery : IFundDetailQuery
    {
        private readonly IDateReader<long, FundDetail> _fundDetailReader;
        private readonly IFundDetailMapper _fundDetailMapper;

        public FundDetailQuery(IDateReader<long, FundDetail> fundDetailReader, IFundDetailMapper fundDetailMapper)
        {
            _fundDetailReader = fundDetailReader;
            _fundDetailMapper = fundDetailMapper;
        }
        public async Task<IList<FundDetailValueObject>> GetAllValueObjectAsync()
        {
            var data = await _fundDetailReader.GetAllAsync();
            return data.Select(_fundDetailMapper.Map).ToList();
        }
        public async Task<IList<FundDetailValueObject>> GetValueObjectsAsync(DateTime date)
        {
            var data = await _fundDetailReader.GetAsync(date);
            return data.Select(_fundDetailMapper.Map).ToList();
        }
    }
}
