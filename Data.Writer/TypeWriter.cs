using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Data.Contract;
using Microsoft.EntityFrameworkCore;

namespace Data.Writer
{
    public interface ITypeWriter<TKey, TType>
            where TType : BaseTypeDao<TKey>
    {
        Task AddAsync(TType item);
        Task UpdateAsync(TType item);
        Task DeleteAsync(TKey id);
        Task AddRangeAsync(IEnumerable<TType> items, bool commitImmediately = false);
    }
    public class TypeWriter<TKey, TType> : ITypeWriter<TKey, TType>
        where TType : BaseTypeDao<TKey>
    {
        private readonly IDbContext _context;

        public TypeWriter(IDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
        public async Task AddAsync(TType item)
        {
            await _context.Set<TType>().AddAsync(item);
        }
        public async Task AddRangeAsync(IEnumerable<TType> items, bool commitImmediately = false)
        {
            await _context.Set<TType>().AddRangeAsync(items);
            if (commitImmediately) await _context.SaveChangesAsync();
        }
        public async Task UpdateAsync(TType item)
        {
            _context.Set<TType>().Update(item);
        }
        public async Task DeleteAsync(TKey id)
        {
            var toBeRemoved = await _context.Set<TType>().FirstOrDefaultAsync(x => Equals(x.Id, id));
            if (!(toBeRemoved is null)) _context.Set<TType>().Remove(toBeRemoved);
        }
    }
}
