using StockService.Application.Abstractions;
using StockService.Domain.Common;
using StockService.Infrastructure.Persistence.Contexts;
using StockService.Infrastructure.Persistence.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockService.Infrastructure.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly StockDbContext _context;

        public UnitOfWork(StockDbContext context)
        {
            _context = context;
        }

        public IRepository<T> GetRepository<T>() where T : class
            => new Repository<T>(_context);

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var result = await _context.SaveChangesAsync(cancellationToken);
            return result;
        }
    }
}
