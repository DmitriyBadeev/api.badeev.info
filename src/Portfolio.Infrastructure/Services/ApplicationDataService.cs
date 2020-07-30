namespace Portfolio.Infrastructure.Services
{
    public class ApplicationDataService
    {
        public ApplicationDataService(AppDbContext appContext)
        {
            EfContext = appContext;
        }

        public AppDbContext EfContext { get; }
    }
}