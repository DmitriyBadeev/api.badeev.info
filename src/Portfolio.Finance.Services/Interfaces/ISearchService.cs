using System.Threading.Tasks;
using Portfolio.Finance.Services.DTO;

namespace Portfolio.Finance.Services.Interfaces
{
    public interface ISearchService
    {
        Task<SearchData> Search(string code);
        Task<AssetData> GetAssetData(string ticket, int userId);
    }
}