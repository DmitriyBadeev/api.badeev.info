using System.Threading.Tasks;
using HotChocolate;
using HotChocolate.AspNetCore.Authorization;
using HotChocolate.Subscriptions;
using HotChocolate.Types;
using Microsoft.Extensions.Logging;
using Portfolio.Finance.API.Services.Interfaces;
using Portfolio.Finance.API.Subscriptions;

namespace Portfolio.Finance.API.Mutations
{
    [ExtendObjectType(Name = "Mutations")]
    public class UpdateMutations
    {
        [Authorize]
        public async Task<string> StartPortfoliosReportUpdate([Service] ITopicEventSender eventSender, [Service] ITimerService timerService,
            [CurrentUserIdGlobalState] int userId, [Service] ILogger<UpdateMutations> logger)
        {
            await eventSender.SendAsync(nameof(ReportSubscriptions.OnUpdatePortfoliosReport), userId);

            var handlerId = timerService.Subscribe((source, args) =>
            {
                logger.LogInformation("Update portfolios report event");
                eventSender.SendAsync(nameof(ReportSubscriptions.OnUpdatePortfoliosReport), userId);
            });

            return handlerId;
        }

        [Authorize]
        public async Task<string> StartAssetReportsUpdate([Service] ITopicEventSender eventSender, [Service] ITimerService timerService,
            [CurrentUserIdGlobalState] int userId, [Service] ILogger<UpdateMutations> logger, int portfolioId)
        {
            var userAndPortfolioIds = new UserAndPortfolioIds()
            {
                PortfolioId = portfolioId,
                UserId = userId
            };

            await eventSender.SendAsync(nameof(ReportSubscriptions.OnUpdateStockReports), userAndPortfolioIds);
            await eventSender.SendAsync(nameof(ReportSubscriptions.OnUpdateFondReports), userAndPortfolioIds);
            await eventSender.SendAsync(nameof(ReportSubscriptions.OnUpdateBondReports), userAndPortfolioIds);

            var handlerId = timerService.Subscribe((source, args) =>
            {
                logger.LogInformation("Update asset reports event");
                eventSender.SendAsync(nameof(ReportSubscriptions.OnUpdateStockReports), userAndPortfolioIds);
                eventSender.SendAsync(nameof(ReportSubscriptions.OnUpdateFondReports), userAndPortfolioIds);
                eventSender.SendAsync(nameof(ReportSubscriptions.OnUpdateBondReports), userAndPortfolioIds);
            });

            return handlerId;
        }

        [Authorize]
        public string StopUpdate([Service] ITimerService timerService, string handlerId)
        {
            timerService.Unsubscribe(handlerId);
            return "Атписка";
        }
    }

    public class UserAndPortfolioIds
    {
        public int UserId { get; set; }
        public int PortfolioId { get; set; }
    }
}
