using System.Collections.Generic;
using Portfolio.Core.Interfaces;

namespace Portfolio.Core.Entities.Finance
{
    public class Portfolio : IEntityBase
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public string Name { get; set; }

        public List<AssetOperation> AssetOperations { get; set; }
    }
}
