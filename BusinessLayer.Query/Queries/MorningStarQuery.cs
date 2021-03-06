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
    public interface IMorningStarQuery
    {
        Task<IList<MorningStarValueObject>> GetValueObjectsAsync(DateTime date);
    }
    public class MorningStarQuery : IMorningStarQuery
    {
        private readonly IDateReader<long, MorningStar> _morningStarReader;
        private readonly IMorningStarMapper _morningStarMapper;

        public MorningStarQuery(IDateReader<long, MorningStar> morningStarReader, IMorningStarMapper morningStarMapper)
        {
            _morningStarReader = morningStarReader;
            _morningStarMapper = morningStarMapper;
        }

        public async Task<IList<MorningStarValueObject>> GetValueObjectsAsync(DateTime date)
        {
            var data = await _morningStarReader.GetAsync(date);
            return data.Select(_morningStarMapper.Map).ToList();
        }
    }
}
