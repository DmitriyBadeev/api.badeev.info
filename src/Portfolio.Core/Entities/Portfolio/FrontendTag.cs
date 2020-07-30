namespace Portfolio.Core.Entities.Portfolio
{
    public class FrontendTag
    {
        public int  Id { get; set; }

        public int TagId { get; set; }

        public Tag Tag { get; set; }
    }
}
