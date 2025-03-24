using NotificationService.Domain.Common;
using NotificationService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationService.Domain.Entities
{
    public class NotificationLog : Entity
    {
        public int CustomerId { get; private set; }
        public string Message { get; private set; } = null!;
        public NotificationChannel Channel { get; private set; }
        public DateTime SentAt { get; private set; }

        private NotificationLog() { }

        private NotificationLog(int customerId, string message, NotificationChannel channel)
        {
            CustomerId = customerId;
            Message = message;
            Channel = channel;
            SentAt = DateTime.UtcNow;
        }

        public static NotificationLog Create(int customerId, string message, NotificationChannel channel)
        {
            return new NotificationLog(customerId, message, channel);
        }
    }
}
