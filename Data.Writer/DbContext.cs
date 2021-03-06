using System;
using System.Threading;
using System.Threading.Tasks;
using Data.Contract;
using Microsoft.EntityFrameworkCore;

namespace Data.Writer
{
    public interface IDbContext
    {
        int SaveChanges();
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
        void Dispose();
        ValueTask DisposeAsync();
        DbSet<TEntity> Set<TEntity>() where TEntity : class;

    }
    public class DbContext : Microsoft.EntityFrameworkCore.DbContext, IDbContext
    {
        public DbSet<Fund> Fund { get; set; }
        public DbSet<FundDetail> FundDetail { get; set; }
        public DbSet<FundCompany> FundCompany { get; set; }
        public DbSet<FundManager> FundManager { get; set; }
        public DbSet<IndexFund> IndexFund { get; set; }
        public DbSet<FundCalculatedData> FundCalculatedData { get; set; }
        public DbSet<MorningStar> MorningStar { get; set; }
        public DbSet<Shareholder> Shareholder { get; set; }
        private readonly string _connectionString;
        public DbContext(string connectionString)
        {
            _connectionString = connectionString;
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                  .UseLazyLoadingProxies()
                  .UseNpgsql(_connectionString)
                  .UseSnakeCaseNamingConvention();
        }
    }
}
