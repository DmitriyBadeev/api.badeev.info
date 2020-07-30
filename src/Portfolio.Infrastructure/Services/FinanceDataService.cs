namespace Portfolio.Infrastructure.Services
{
    public class FinanceDataService
    {
        public FinanceDataService(FinanceDbContext efFinanceContext)
        {
            EfContext = efFinanceContext;
        }

        public FinanceDbContext EfContext { get; }
    }
}
