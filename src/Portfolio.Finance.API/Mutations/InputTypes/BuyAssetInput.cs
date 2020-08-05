using System;

namespace Portfolio.Finance.API.Mutations.InputTypes
{
    public class BuyAssetInput
    {
        public int PortfolioId { get; set; }

        public string Ticket { get; set; }
        
        public int Price { get; set; } 
        
        public int Amount { get; set; }

        public int AssetTypeId { get; set; } 
        
        public DateTime Date { get; set; }
    }
}