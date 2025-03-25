using Microsoft.EntityFrameworkCore;
using NotificationService.Application.Abstractions;
using NotificationService.Domain.Entities;
using NotificationService.Domain.Enums;
using NotificationService.Infrastructure.Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationService.Infrastructure.Persistence.Repositories
{
    public class NotificationLogRepository : Repository<NotificationLog>, INotificationLogRepository
    {
        private readonly NotificationDbContext _context;

        public NotificationLogRepository(NotificationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<NotificationLog>> GetByCustomerIdAsync(int customerId)
        {
            return await _context.NotificationLogs
                .Where(x => x.CustomerId == customerId)
                .ToListAsync();
        }

        public async Task<List<NotificationLog>> GetFailedNotificationsAsync()
        {
            return await _context.Set<NotificationLog>()
                .Where(n => n.Status == NotificationStatus.Failed)
                .ToListAsync();
        }
    }
}
