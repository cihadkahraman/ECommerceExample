using Microsoft.EntityFrameworkCore;
using StockService.Application.Abstractions;
using StockService.Domain.Entities;
using StockService.Infrastructure.Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockService.Infrastructure.Persistence.Repositories
{
    public class StockRepository : Repository<Stock>, IStockRepository
    {
        private readonly StockDbContext _context;

        public StockRepository(StockDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Stock?> GetByProductIdAsync(int productId)
        {
            return await _context.Stocks
                .Include(s => s.Product)
                .FirstOrDefaultAsync(s => s.ProductId == productId);
        }
    }
}
