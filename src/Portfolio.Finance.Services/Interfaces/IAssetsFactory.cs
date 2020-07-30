using System.Collections.Generic;

namespace Portfolio.Finance.Services.Interfaces
{
    public interface IAssetsFactory
    {
        List<IAssetInfo> Create();
    }
}