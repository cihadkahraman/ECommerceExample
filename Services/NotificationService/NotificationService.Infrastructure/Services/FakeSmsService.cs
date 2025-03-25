using Microsoft.Extensions.Logging;
using NotificationService.Application.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationService.Infrastructure.Services
{
    public class FakeSmsService : ISmsService
    {
        private readonly ILogger<FakeSmsService> _logger;

        public FakeSmsService(ILogger<FakeSmsService> logger)
        {
            _logger = logger;
        }

        public Task SendSmsAsync(string phoneNumber, string message)
        {
            _logger.LogInformation("Fake SMS gönderildi -> {PhoneNumber} : {Message}", phoneNumber, message);
            return Task.CompletedTask;
        }
    }
}
