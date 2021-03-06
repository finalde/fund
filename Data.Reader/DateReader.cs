using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Common.Extensions;
using Dapper;
using Data.Contract;

namespace Data.Reader
{
    public interface IDateReader<TKey, TDate>
        where TDate : BaseDateDao<TKey>
    {
        Task<IList<TDate>> GetAllAsync();
        Task<IList<TDate>> GetAsync(DateTime date);
    }
    public class DateReader<TKey, TDate> : IDateReader<TKey, TDate>
              where TDate : BaseDateDao<TKey>
    {
        private readonly IDbConnection _connection;

        public DateReader(IDbConnection connection)
        {
            _connection = connection;
        }
        public async Task<IList<TDate>> GetAsync(DateTime date)
        {
            var data = (await _connection.QueryAsync<TDate>($"select * from {typeof(TDate).Name.FromPascalToSnakeCase()} where date=@date", new { date })).ToList();
            return data;
        }
        public async Task<IList<TDate>> GetAllAsync()
        {
            var data = (await _connection.QueryAsync<TDate>($"select * from {typeof(TDate).Name.FromPascalToSnakeCase()}")).ToList();
            return data;
        }
    }
}
