using System.Collections.Generic;
using Portfolio.Core.Interfaces;

namespace Portfolio.Core.Entities
{
    public class Tag : IEntityBase
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public IEnumerable<TagWork> Works { get; set; }
    }
}
