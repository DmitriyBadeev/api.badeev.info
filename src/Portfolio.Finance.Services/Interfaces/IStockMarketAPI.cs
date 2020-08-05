using System;
using System.Threading.Tasks;
using Portfolio.Finance.Services.Entities;

namespace Portfolio.Finance.Services.Interfaces
{
    public interface IStockMarketAPI
    {
        Task<ApiResponse> FindStock(string codeStock);
        Task<ApiResponse> FindFond(string codeFond);
        Task<ApiResponse> FindBond(string codeBond);
        Task<ApiResponse> FindDividends(string codeStock);
        Task<ApiResponse> FindCoupons(string codeBond, DateTime boughtDate);
    }
}