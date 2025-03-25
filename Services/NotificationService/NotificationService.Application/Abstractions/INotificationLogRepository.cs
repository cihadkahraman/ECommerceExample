using NotificationService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationService.Application.Abstractions
{
    public interface INotificationLogRepository : IRepository<NotificationLog>
    {
        Task<List<NotificationLog>> GetByCustomerIdAsync(int customerId);
        Task<List<NotificationLog>> GetFailedNotificationsAsync();
    }
}
