using System.Collections.Generic;
using Portfolio.Finance.Services.Entities;

namespace Portfolio.Finance.Services.Interfaces
{
    public interface IAssetsFactory
    {
        List<AssetInfo> Create(int portfolioId);
    }
}