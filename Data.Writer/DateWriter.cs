using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Data.Contract;
using Microsoft.EntityFrameworkCore;

namespace Data.Writer
{
    public interface IDateWriter<TKey, TDate>
            where TDate : BaseDateDao<TKey>
    {
        Task AddAsync(TDate item);
        Task UpdateAsync(TDate item);
        Task DeleteAsync(TKey id);
        Task AddRangeAsync(IEnumerable<TDate> items, bool commitImmediately = false);
    }
    public class DateWriter<TKey, TDate> : IDateWriter<TKey, TDate>
        where TDate : BaseDateDao<TKey>
    {
        private readonly IDbContext _context;

        public DateWriter(IDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
        public async Task AddAsync(TDate item)
        {
            await _context.Set<TDate>().AddAsync(item);
        }
        public async Task AddRangeAsync(IEnumerable<TDate> items, bool commitImmediately = false)
        {
            await _context.Set<TDate>().AddRangeAsync(items);
            if (commitImmediately) await _context.SaveChangesAsync();
        }
        public async Task UpdateAsync(TDate item)
        {
            _context.Set<TDate>().Update(item);
        }
        public async Task DeleteAsync(TKey id)
        {
            var toBeRemoved = await _context.Set<TDate>().FirstOrDefaultAsync(x => Equals(x.Id, id));
            if (!(toBeRemoved is null)) _context.Set<TDate>().Remove(toBeRemoved);
        }
    }
}
