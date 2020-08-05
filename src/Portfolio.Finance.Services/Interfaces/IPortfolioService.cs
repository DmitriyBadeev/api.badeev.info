using System.Collections.Generic;
using System.Threading.Tasks;
using Portfolio.Finance.Services.Entities;

namespace Portfolio.Finance.Services.Interfaces
{
    public interface IPortfolioService
    {
        Task<OperationResult> CreatePortfolio(string name, int userId);

        IEnumerable<Core.Entities.Finance.Portfolio> GetPortfolios(int userId);
    }
}