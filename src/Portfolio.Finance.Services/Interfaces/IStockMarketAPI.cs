using System;
using System.Threading.Tasks;
using Portfolio.Finance.Services.DTO;
using Portfolio.Finance.Services.DTO.Responses;

namespace Portfolio.Finance.Services.Interfaces
{
    public interface IStockMarketAPI
    {
        Task<ApiResponse> FindStock(string codeStock);
        Task<ApiResponse> FindFond(string codeFond);
        Task<ApiResponse> FindBond(string codeBond);
        Task<ApiResponse> FindIndex(string codeIndex);
        Task<ApiResponse> FindCurrency(string codeCurrency);
        Task<ApiResponse> FindBrent();
        Task<ApiResponse> FindDividends(string codeStock);
        Task<ApiResponse> FindCoupons(string codeBond, DateTime boughtDate);
        Task<ApiResponse> Search(string code);
    }
}