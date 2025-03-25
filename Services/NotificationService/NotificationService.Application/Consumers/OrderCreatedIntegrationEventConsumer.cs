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
        private readonly ISmsService _smsService;
        private readonly IUnitOfWork _unitOfWork;

        public OrderCreatedIntegrationEventConsumer(
            INotificationSenderService notificationSender,
            ILogger<OrderCreatedIntegrationEventConsumer> logger,
            IEmailService emailService,
            ISmsService smsService,
            ICustomerRepository customerRepository,
            IUnitOfWork unitOfWork)
        {
            _notificationSender = notificationSender;
            _logger = logger;
            _emailService = emailService;
            _smsService = smsService;
            _customerRepository = customerRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task Consume(ConsumeContext<OrderCreatedIntegrationEvent> context)
        {
            var message = context.Message;
            var correlationId = new CorrelationId(context.Headers.GetCorrelationId().Value);

            var customer = await _customerRepository.GetByIdAsync(message.CustomerId);

            if (customer is null)
            {
                _logger.LogError($"Müşteri bulunamadı: {message.CustomerId}");
                throw new Exception("Müşteri bulunamadı");
            }

            
            var payload = new OrderCreatedLogPayload(message.OrderId, message.CustomerId, message.CreatedAt, message.Items);

            var emailNotification = NotificationLog.Create(
                customer.Id,
                $"Dear customer, your order has been successfully completed.",
                NotificationChannel.Email
            );

            _logger.LogInformationWithPayload($"{customer.Id} numaralı müşteri logu kaydediliyor", correlationId, payload);

            await _notificationSender.SendNotificationAsync(emailNotification);
            await _unitOfWork.SaveChangesAsync();

            try
            {
                await _emailService.SendEmailAsync(customer.Email, "Order Confirmation", emailNotification.Message);
                emailNotification.Status = NotificationStatus.Sent;
            }
            catch (Exception ex)
            {
                _logger.LogError("Mail gönderimi başarısız oldu. SMS'e geçiliyor...");
                emailNotification.Status = NotificationStatus.Failed;

                var smsNotification = NotificationLog.Create(
                    customer.Id,
                    emailNotification.Message,
                    NotificationChannel.Sms
                );
                await _notificationSender.SendNotificationAsync(smsNotification);

                try
                {
                    await _smsService.SendSmsAsync(customer.PhoneNumber, smsNotification.Message);
                    smsNotification.Status = NotificationStatus.Sent;
                    _logger.LogInformation("SMS fallback başarılı!");
                }
                catch (Exception smsEx)
                {
                    smsNotification.Status = NotificationStatus.Failed;
                    _logger.LogError(smsEx, "SMS gönderimi de başarısız oldu.");
                }
            }

            await _unitOfWork.SaveChangesAsync();
        }
    }
}
