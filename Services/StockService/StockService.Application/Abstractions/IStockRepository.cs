using StockService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockService.Application.Abstractions
{
    public interface IStockRepository : IRepository<Stock>
    {
        Task<Stock?> GetByProductIdAsync(int productId);
    }
}
