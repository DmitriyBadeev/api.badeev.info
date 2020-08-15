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
            await eventSender.SendAsync(nameof(ReportSubscriptions.OnUpdatePricesReport), userId);

            var handlerId = timerService.Subscribe((source, args) =>
            {
                logger.LogInformation("Update portfolios report event");
                eventSender.SendAsync(nameof(ReportSubscriptions.OnUpdatePortfoliosReport), userId);
                eventSender.SendAsync(nameof(ReportSubscriptions.OnUpdatePricesReport), userId);
            });

            return handlerId;
        }

        [Authorize]
        public async Task<string> StartAssetReportsUpdate([Service] ITopicEventSender eventSender, [Service] ITimerService timerService,
            [CurrentUserIdGlobalState] int userId, [Service] ILogger<UpdateMutations> logger)
        {
            await eventSender.SendAsync(nameof(ReportSubscriptions.OnUpdateStockReports), userId);
            await eventSender.SendAsync(nameof(ReportSubscriptions.OnUpdateFondReports), userId);
            await eventSender.SendAsync(nameof(ReportSubscriptions.OnUpdateBondReports), userId);

            var handlerId = timerService.Subscribe((source, args) =>
            {
                logger.LogInformation("Update asset reports event");
                eventSender.SendAsync(nameof(ReportSubscriptions.OnUpdateStockReports), userId);
                eventSender.SendAsync(nameof(ReportSubscriptions.OnUpdateFondReports), userId);
                eventSender.SendAsync(nameof(ReportSubscriptions.OnUpdateBondReports), userId);
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
}
