using Microsoft.EntityFrameworkCore;
using Portfolio.Core.Entities.Finance;

namespace Portfolio.Infrastructure
{
    public class FinanceDbContext : DbContext
    {
        public DbSet<Core.Entities.Finance.Portfolio> Portfolios { get; set; }
        public DbSet<AssetOperation> AssetOperations { get; set; }
        public DbSet<AssetAction> AssetActions { get; set; }
        public DbSet<AssetType> AssetTypes { get; set; }
        public DbSet<CurrencyOperation> CurrencyOperations { get; set; }
        public DbSet<CurrencyAction> CurrencyActions { get; set; }
    
        public DbSet<Payment> Payments { get; set; }
        public FinanceDbContext(DbContextOptions<FinanceDbContext> options) : base(options)
        {
        }
    }
}
