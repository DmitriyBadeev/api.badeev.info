using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Logging;
using Portfolio.Core.Interfaces;

namespace Portfolio.Infrastructure.Services
{
    public class SeedDataService : ISeedDataService
    {
        private readonly ILogger<SeedDataService> _logger;
        private readonly AppDbContext _context;

        public SeedDataService(ILogger<SeedDataService> logger, AppDbContext context)
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
                

            _logger.LogInformation("Database has seeded successfully");
        }
    }
}
