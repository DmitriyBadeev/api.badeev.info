using System.Collections.Generic;
using Portfolio.Core.Interfaces;

namespace Portfolio.Core.Entities.Finance
{
    public class CurrencyAction : IEntityBase
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public List<CurrencyOperation> CurrencyOperations { get; set; }
    }
}