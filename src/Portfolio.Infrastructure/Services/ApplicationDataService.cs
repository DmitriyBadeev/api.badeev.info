namespace Portfolio.Infrastructure.Services
{
    public class ApplicationDataService : IDataService
    {
        public ApplicationDataService(AppDbContext appContext)
        {
            EfContext = appContext;
        }

        public AppDbContext EfContext { get; }
    }
}