using NotificationService.Domain.Common;
using NotificationService.Domain.Enums;

namespace NotificationService.Domain.Entities
{
    public class NotificationLog : Entity
    {
        public int CustomerId { get; private set; }
        public string Message { get; private set; } = null!;
        public NotificationChannel Channel { get; private set; }
        public NotificationStatus Status { get; set; }
        public DateTime SentAt { get; private set; }

        private NotificationLog() { }

        private NotificationLog(int customerId, string message, NotificationChannel channel, NotificationStatus notificationStatus)
        {
            CustomerId = customerId;
            Message = message;
            Channel = channel;
            SentAt = DateTime.UtcNow;
            Status = notificationStatus;
        }

        public static NotificationLog Create(int customerId, string message, NotificationChannel channel)
        {
            return new NotificationLog(customerId, message, channel, NotificationStatus.Pending);
        }
    }
}
