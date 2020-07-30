namespace Portfolio.Infrastructure.Services
{
    public class FinanceDataService : IDataService
    {
        public FinanceDataService(AppDbContext efFinanceContext)
        {
            EfContext = efFinanceContext;
        }

        public AppDbContext EfContext { get; }
    }
}
