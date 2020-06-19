namespace Portfolio.Infrastructure.Services
{
    public class ApplicationDataService
    {
        public ApplicationDataService(AppDbContext context)
        {
            EfContext = context;
        }

        public AppDbContext EfContext { get; }
    }
}