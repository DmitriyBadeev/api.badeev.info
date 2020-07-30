using System;
using Portfolio.Core.Interfaces;

namespace Portfolio.Core.Entities.Finance
{
    public class AssetOperation : IEntityBase
    {
        public int Id { get; set; }

        public string Ticket { get; set; }

        public int Amount { get; set; }

        public int PaymentPrice { get; set; }

        public DateTime Date { get; set; }

        public Portfolio Portfolio { get; set; }

        public int PortfolioId { get; set; }

        public AssetType AssetType { get; set; }

        public int AssetTypeId { get; set; }

        public AssetAction AssetAction { get; set; }

        public int AssetActionId { get; set; }
    }
}
