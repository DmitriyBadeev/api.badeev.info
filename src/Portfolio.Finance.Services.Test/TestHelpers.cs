using System;
using Microsoft.EntityFrameworkCore;
using Portfolio.Infrastructure;

namespace Portfolio.Finance.Services.Test
{
    public static class TestHelpers
    {
        public static FinanceDbContext GetMockFinanceDbContext()
        {
            var options = new DbContextOptionsBuilder<FinanceDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            return new FinanceDbContext(options);
        }
    }
}