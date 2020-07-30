using Portfolio.Core.Entities.Finance;

namespace Portfolio.Finance.Services.Interfaces
{
    public interface IAssetInfo
    {
        string Ticket { get; }

        int Amount { get; }

        int BoughtPrice { get; }

        void RegisterOperation(AssetOperation operation);
    }
}