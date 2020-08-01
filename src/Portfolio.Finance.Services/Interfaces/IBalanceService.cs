namespace Portfolio.Finance.Services.Interfaces
{
    public interface IBalanceService
    {
        int GetBalance(int portfolioId);

        int GetAllBalanceUser(int userId);
    }
}