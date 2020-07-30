namespace Portfolio.Infrastructure.Services
{
    public interface IDataService
    {
        AppDbContext EfContext { get; }
    }
}