using System.Threading.Tasks;
using HotChocolate;
using HotChocolate.AspNetCore.Authorization;
using HotChocolate.Types;
using Portfolio.Finance.API.Mutations.InputTypes;
using Portfolio.Finance.Services.DTO;
using Portfolio.Finance.Services.Interfaces;

namespace Portfolio.Finance.API.Mutations
{
    [ExtendObjectType(Name = "Mutations")]
    public class PortfolioMutations
    {
        [Authorize]
        public async Task<OperationResult> CreatePortfolio([CurrentUserIdGlobalState] int userId,
            [Service] IPortfolioService portfolioService, string name)
        {
            var result = await portfolioService.CreatePortfolio(name, userId);

            return result;
        }

        [Authorize]
        public async Task<OperationResult> AddPaymentInPortfolio([CurrentUserIdGlobalState] int userId,
            [Service] IPortfolioService portfolioService, PaymentInput paymentInput)
        {
            var result = await portfolioService.AddPaymentInPortfolio(paymentInput.PortfolioId, userId,
                paymentInput.Ticket, paymentInput.Amount, paymentInput.PaymentValue, paymentInput.Date);

            return result;
        }
    }
}