namespace Portfolio.Finance.API.Queries.Response
{
    public class AllPortfoliosReport
    {
        public double AllCost { get; set; }
        public double AllPaperProfit { get; set; }
        public double AllPaperProfitPercent { get; set; }
        public double AllPaymentProfitPercent { get; set; }
        public double AllPaymentProfit { get; set; }
        public double AllInvestSum { get; set; }
        public double AllUserBalance { get; set; }
    }
}