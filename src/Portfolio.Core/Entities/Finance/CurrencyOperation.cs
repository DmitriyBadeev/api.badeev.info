using System;
using Portfolio.Core.Interfaces;

namespace Portfolio.Core.Entities.Finance
{
    public class CurrencyOperation : IEntityBase
    {
        public int Id { get; set; }

        public string CurrencyName { get; set; }

        public int CurrencyId { get; set; }

        public int Price { get; set; }

        public DateTime Date { get; set; }

        public int CurrencyActionId { get; set; }

        public CurrencyAction CurrencyAction { get; set; }

        public int PortfolioId { get; set; }

        public Portfolio Portfolio { get; set; }
    }
}
