using MassTransit;
using Microsoft.Extensions.Logging;
using NotificationService.Application.Common.Logging;
using NotificationService.Application.Common.Models;
using NotificationService.Application.Common.Serialization;
using NotificationService.Application.Events;
using NotificationService.Application.Services;
using NotificationService.Domain.Entities;
using NotificationService.Domain.Enums;
using System.Text.Json;

namespace NotificationService.Application.Consumers
{
    public class OrderCreatedIntegrationEventConsumer : IConsumer<OrderCreatedIntegrationEvent>
    {
        private readonly INotificationSenderService _notificationSender;
        private readonly ILogger<OrderCreatedIntegrationEventConsumer> _logger;

        public OrderCreatedIntegrationEventConsumer(INotificationSenderService notificationSender, ILogger<OrderCreatedIntegrationEventConsumer> logger)
        {
            _notificationSender = notificationSender;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<OrderCreatedIntegrationEvent> context)
        {
            var message = context.Message;

            var notification = NotificationLog.Create(
                message.CustomerId,
                $"Dear customer, your order has been successfully completed.",
                NotificationChannel.Email
            );
            var customerId = message.CustomerId;
            var correlationId = new CorrelationId(context.Headers.GetCorrelationId().Value);
            var payload = new OrderCreatedLogPayload(message.OrderId, message.CustomerId, message.CreatedAt, message.Items);

            _logger.LogInformationWithPayload($"{customerId} numaralı müşteriye log gönderiliyor", correlationId, payload);
            await _notificationSender.SendNotificationAsync(notification);
        }
    }
}
