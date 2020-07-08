namespace Portfolio.Core.Entities
{
    public class BackendTag
    {
        public int Id { get; set; }

        public int TagId { get; set; }

        public Tag Tag { get; set; }
    }
}
