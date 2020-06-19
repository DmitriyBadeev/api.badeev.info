using Portfolio.Core.Interfaces;

namespace Portfolio.Core.Entities
{
    public class Author : IEntityBase
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Role { get; set; }

        public string Link { get; set; }

        public int WorkId { get; set; }

        public Work Work { get; set; }
    }
}
