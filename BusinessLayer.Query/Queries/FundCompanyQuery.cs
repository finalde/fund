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
    public interface IFundCompanyQuery
    {
        Task<IList<FundCompanyValueObject>> GetValueObjectsAsync(DateTime date);
    }
    public class FundCompanyQuery: IFundCompanyQuery
    {
        private readonly IDateReader<long, FundCompany> _fundCompanyReader;
        private readonly IFundCompanyMapper _fundCompanyMapper;

        public FundCompanyQuery(IDateReader<long, FundCompany> fundCompanyReader, IFundCompanyMapper fundCompanyMapper)
        {
            _fundCompanyReader = fundCompanyReader ?? throw new ArgumentNullException(nameof(fundCompanyReader));
            _fundCompanyMapper = fundCompanyMapper ?? throw new ArgumentNullException(nameof(fundCompanyMapper));
        }

        public async Task<IList<FundCompanyValueObject>> GetValueObjectsAsync(DateTime date)
        {
            var data = await _fundCompanyReader.GetAsync(date);
            return data.Select(_fundCompanyMapper.Map).ToList();
        }
    }
}
