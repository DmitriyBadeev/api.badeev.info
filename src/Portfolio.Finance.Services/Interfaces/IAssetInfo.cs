using System.Collections.Generic;
using System.Threading.Tasks;
using Portfolio.Core.Entities.Finance;
using Portfolio.Finance.Services.Entities;

namespace Portfolio.Finance.Services.Interfaces
{
    public interface IAssetInfo
    {
        string Ticket { get; }

        int Amount { get; }

        int BoughtPrice { get; }

        Task<string> GetName();

        List<PaymentData> PaymentsData { get; }

        List<PaymentData> GetFuturePayment();

        Task<int> GetPrice();

        Task<int> GetPaperProfit();

        List<PaymentData> GetPaidPayments();

        int GetSumPayments();

        void RegisterOperation(AssetOperation operation);
    }
}