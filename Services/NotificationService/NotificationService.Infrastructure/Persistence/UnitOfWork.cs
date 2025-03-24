using NotificationService.Application.Abstractions;
using NotificationService.Infrastructure.Persistence.Contexts;
using NotificationService.Infrastructure.Persistence.Repositories;

namespace NotificationService.Infrastructure.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly NotificationDbContext _context;

        public UnitOfWork(NotificationDbContext context)
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
