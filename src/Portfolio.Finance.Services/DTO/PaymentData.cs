using System;

namespace Portfolio.Finance.Services.DTO
{
    public class PaymentData
    {
        public string Ticket { get; set; }

        public int PaymentValue { get; set; }

        public DateTime RegistryCloseDate { get; set; }

        public string CurrencyId { get; set; }
    }
}
