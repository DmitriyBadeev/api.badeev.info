﻿using System.Threading.Tasks;
using Portfolio.Finance.Services.Entities;

namespace Portfolio.Finance.Services.Interfaces
{
    public interface IStockMarketAPI
    {
        Task<ApiResponse> FindStock(string codeStock);
    }
}