using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Portfolio.Core.Entities.Finance;
using Portfolio.Core.Interfaces;

namespace Portfolio.Infrastructure.Services
{
    public class SeedFinanceDataService : ISeedDataService
    {
        private readonly ILogger<SeedAppDataService> _logger;
        private readonly FinanceDbContext _context;

        public SeedFinanceDataService(ILogger<SeedAppDataService> logger, FinanceDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public void Initialise()
        {
            _logger.LogInformation("Seeding database");

            if (_context.Database.GetPendingMigrations().Any())
            {
                _logger.LogInformation("Migrating database");
                _context.Database.Migrate();
                _logger.LogInformation("Database has migrated successfully");
            }

            AddActions();
            AddAssetTypes();

            _logger.LogInformation("Database has seeded successfully");
        }

        private void AddActions()
        {
            if (!_context.AssetActions.Any())
            {
                _logger.LogInformation("Add asset actions");
                var buy = new AssetAction()
                {
                    Name = "Покупка"
                };

                var sell = new AssetAction()
                {
                    Name = "Продажа"
                };

                _context.AssetActions.AddRange(buy, sell);
                _context.SaveChanges();
                _logger.LogInformation("Added asset actions successfully");
            }

            if (!_context.CurrencyActions.Any())
            {
                _logger.LogInformation("Add currency actions");
                var refill = new CurrencyAction()
                {
                    Name = "Пополнение"
                };

                var withdrawal = new CurrencyAction()
                {
                    Name = "Вывод"
                };

                _context.CurrencyActions.AddRange(refill, withdrawal);
                _context.SaveChanges();
                _logger.LogInformation("Added currency actions successfully");
            }
        }

        private void AddAssetTypes()
        {
            if (!_context.AssetTypes.Any())
            {
                _logger.LogInformation("Add asset types");
                var stock = new AssetType()
                {
                    Name = "Акция"
                };

                var fond = new AssetType()
                {
                    Name = "Фонд"
                };

                var bond = new AssetType()
                {
                    Name = "Облигация"
                };

                _context.AssetTypes.AddRange(stock, fond, bond);
                _context.SaveChanges();
                _logger.LogInformation("Added asset types successfully");
            }
        }
    }
}