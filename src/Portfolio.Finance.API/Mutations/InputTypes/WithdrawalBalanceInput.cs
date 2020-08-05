using System;

namespace Portfolio.Finance.API.Mutations.InputTypes
{
    public class WithdrawalBalanceInput
    {
        public int PortfolioId { get; set; }

        public int Price { get; set; }

        public DateTime Date { get; set; }
    }
}