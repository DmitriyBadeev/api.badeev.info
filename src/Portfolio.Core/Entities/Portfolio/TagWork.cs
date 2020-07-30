using Portfolio.Core.Interfaces;

namespace Portfolio.Core.Entities.Portfolio
{
    public class TagWork : IEntityBase
    {
        public int Id { get; set; }

        public int WorkId { get; set; }

        public Work Work { get; set; }

        public int TagId { get; set; }

        public Tag Tag { get; set; }
    }
}
