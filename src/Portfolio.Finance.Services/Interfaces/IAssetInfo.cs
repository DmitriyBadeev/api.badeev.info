using System.Collections.Generic;
using Portfolio.Core.Entities.Finance;
using Portfolio.Finance.Services.Entities;

namespace Portfolio.Finance.Services.Interfaces
{
    public interface IAssetInfo
    {
        string Ticket { get; }

        int Amount { get; }

        string Name { get; }

        int BoughtPrice { get; }

        List<PaymentData> PaymentsData { get; }

        List<PaymentData> GetFuturePayment();

        int GetPrice();

        int GetPaperProfit();

        List<PaymentData> GetPaidPayments();

        void RegisterOperation(AssetOperation operation);
    }
}