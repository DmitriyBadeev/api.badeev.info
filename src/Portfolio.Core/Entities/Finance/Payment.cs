using System;
using Portfolio.Core.Interfaces;

namespace Portfolio.Core.Entities.Finance
{
    public class Payment : IEntityBase
    {
        public int Id { get; set; }

        public string Ticket { get; set; }

        public int Amount { get; set; }

        public int PaymentValue { get; set; }

        public DateTime Date { get; set; }

        public int PortfolioId { get; set; }

        public Portfolio Portfolio { get; set; }
    }
}