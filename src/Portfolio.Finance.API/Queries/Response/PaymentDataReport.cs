using System;

namespace Portfolio.Finance.API.Queries.Response
{
    public class PaymentDataReport
    {
        public string Name { get; set; }

        public string Ticket { get; set; }

        public double PaymentValue { get; set; }

        public int Amount { get; set; }

        public double AllPayment { get; set; }

        public DateTime RegistryCloseDate { get; set; }

        public string CurrencyId { get; set; }
    }
}