using MassTransit;
using Microsoft.Extensions.Logging;
using NotificationService.Application.Abstractions;
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
        private readonly IEmailService _emailService;
        private readonly ICustomerRepository _customerRepository;

        public OrderCreatedIntegrationEventConsumer(INotificationSenderService notificationSender, ILogger<OrderCreatedIntegrationEventConsumer> logger, IEmailService emailService, ICustomerRepository customerRepository)
        {
            _notificationSender = notificationSender;
            _logger = logger;
            _emailService = emailService;
            _customerRepository = customerRepository;
        }

        public async Task Consume(ConsumeContext<OrderCreatedIntegrationEvent> context)
        {
            var message = context.Message;

            var customer = await _customerRepository.GetByIdAsync(message.CustomerId);

            if (customer is not null)
            {
                await _emailService.SendEmailAsync(customer.Email, "Deneme", $"Dear customer, your order has been successfully completed.");
            }
            else
            {
                _logger.LogError("Müşteri bulunamadı: {CustomerId}", message.CustomerId);
            }
            

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
