using NotificationService.Application.Abstractions;
using NotificationService.Domain.Entities;
using NotificationService.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationService.Application.Services
{
    public class NotificationSenderService : INotificationSenderService
    {
        private readonly INotificationLogRepository _notificationLogRepository;

        public NotificationSenderService(
            INotificationLogRepository notificationLogRepository)
        {
            _notificationLogRepository = notificationLogRepository;
        }

        public async Task SendNotificationAsync(NotificationLog notificationLog)
        {
            await _notificationLogRepository.AddAsync(notificationLog);
        }
    }
}
