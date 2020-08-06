using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Portfolio.Finance.Services.DTO;
using Portfolio.Finance.Services.Interfaces;
using Portfolio.Infrastructure.Services;

namespace Portfolio.Finance.Services.Services
{
    public class PortfolioService : IPortfolioService
    {
        private readonly FinanceDataService _financeData;

        public PortfolioService(FinanceDataService financeData)
        {
            _financeData = financeData;
        }

        public IEnumerable<Core.Entities.Finance.Portfolio> GetPortfolios(int userId)
        {
            return _financeData.EfContext.Portfolios.Where(p => p.UserId == userId);
        }

        public async Task<OperationResult> CreatePortfolio(string name, int userId)
        {
            var portfolio = new Core.Entities.Finance.Portfolio()
            {
                Name = name,
                UserId = userId
            };

            var containsSameNamePortfolio = 
                await _financeData.EfContext.Portfolios.AnyAsync(p => p.Name == name && p.UserId == userId);
            if (containsSameNamePortfolio)
            {
                return new OperationResult()
                {
                    IsSuccess = false,
                    Message = "Порфель с таким именем у Вас уже существует"
                };
            }

            await _financeData.EfContext.Portfolios.AddAsync(portfolio);
            await _financeData.EfContext.SaveChangesAsync();

            return new OperationResult()
            {
                IsSuccess = true,
                Message = $"Портфель {name} создан успешно"
            };
        }
    }
}