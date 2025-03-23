using OrderService.Application.Abstractions;
using OrderService.Domain.Common;
using OrderService.Infrastructure.Persistence.Contexts;
using OrderService.Infrastructure.Persistence.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderService.Infrastructure.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly OrderDbContext _context;
        private readonly IDomainEventDispatcher _eventDispatcher;

        public UnitOfWork(OrderDbContext context, IDomainEventDispatcher eventDispatcher)
        {
            _context = context;
            _eventDispatcher = eventDispatcher;
        }

        public IRepository<T> GetRepository<T>() where T : class
            => new Repository<T>(_context);

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            //var domainEntities = _context.ChangeTracker
            //    .Entries<AggregateRoot>()
            //    .Where(e => e.Entity.DomainEvents.Any())
            //    .ToList();

            //var domainEvents = domainEntities
            //    .SelectMany(e => e.Entity.DomainEvents)
            //    .ToList();

            //await _eventDispatcher.DispatchAsync(domainEvents);

            //foreach (var entity in domainEntities)
            //    entity.Entity.ClearDomainEvents();

            

            var result = await _context.SaveChangesAsync(cancellationToken);

            

            return result;
        }
    }
}
