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
        private readonly ICustomerRepository _customerRepository;
        private readonly INotificationLogRepository _notificationLogRepository;
        private readonly IUnitOfWork _unitOfWork;

        public NotificationSenderService(
            ICustomerRepository customerRepository,
            INotificationLogRepository notificationLogRepository,
            IUnitOfWork unitOfWork)
        {
            _customerRepository = customerRepository;
            _notificationLogRepository = notificationLogRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task SendNotificationAsync(NotificationLog notificationLog)
        {
            var customer = await _customerRepository.GetByIdAsync(notificationLog.CustomerId);

            if (customer == null)
                throw new DomainException("Müşteri bulunamadı.");

            Console.WriteLine($"Sending {notificationLog.Channel} to {customer.Email ?? customer.PhoneNumber}: {notificationLog.Message}");

            var log = NotificationLog.Create
                (
                    notificationLog.CustomerId,
                    notificationLog.Message,
                    notificationLog.Channel
                );

            await _notificationLogRepository.AddAsync(log);

            await _unitOfWork.SaveChangesAsync();
        }
    }
}
