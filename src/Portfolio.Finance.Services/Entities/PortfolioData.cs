using System.Collections.Generic;
using Portfolio.Finance.Services.Interfaces;

namespace Portfolio.Finance.Services.Entities
{
    public class PortfolioData
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int UserId { get; set; }

        public List<IAssetInfo> Assets { get; set; }
    }
}
