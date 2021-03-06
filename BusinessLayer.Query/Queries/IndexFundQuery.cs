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
    public interface IIndexFundQuery
    {
        Task<IList<IndexFundValueObject>> GetValueObjectsAsync(DateTime date);
        Task<IList<IndexFundValueObject>> GetAllValueObjectsAsync();
    }
    public class IndexFundQuery : IIndexFundQuery
    {
        private readonly IDateReader<long, IndexFund> _indexFundReader;
        private readonly IIndexFundMapper _indexFundMapper;

        public IndexFundQuery(IDateReader<long, IndexFund> indexFundReader, IIndexFundMapper indexFundMapper)
        {
            _indexFundReader = indexFundReader;
            _indexFundMapper = indexFundMapper;
        }

        public async Task<IList<IndexFundValueObject>> GetAllValueObjectsAsync()
        {
            var data = await _indexFundReader.GetAllAsync();
            return data.Select(_indexFundMapper.Map).ToList();
        }
        public async Task<IList<IndexFundValueObject>> GetValueObjectsAsync(DateTime date)
        {
            var data = await _indexFundReader.GetAsync(date);
            return data.Select(_indexFundMapper.Map).ToList();
        }
    }
}
