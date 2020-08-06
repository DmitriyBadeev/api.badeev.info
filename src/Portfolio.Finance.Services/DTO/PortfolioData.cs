using System.Collections.Generic;
using Portfolio.Finance.Services.Entities;

namespace Portfolio.Finance.Services.DTO
{
    public class PortfolioData
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int UserId { get; set; }

        public IEnumerable<AssetInfo> Assets { get; set; }
    }
}
