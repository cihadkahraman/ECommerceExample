using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NotificationService.Application.Abstractions;
using NotificationService.Application.Services;
using NotificationService.Domain.Entities;
using NotificationService.Domain.Enums;

namespace NotificationService.Infrastructure.BackgroundJobs
{
    public class FailedNotificationRetryJob : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<FailedNotificationRetryJob> _logger;

        public FailedNotificationRetryJob(IServiceScopeFactory scopeFactory, ILogger<FailedNotificationRetryJob> logger)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken);

                using var scope = _scopeFactory.CreateScope();
                var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
                var failedLogs = await unitOfWork.NotificationLogs.GetFailedNotificationsAsync();

                var notificationSender = scope.ServiceProvider.GetRequiredService<INotificationSenderService>();

                foreach (var log in failedLogs)
                {
                    try
                    {
                        await notificationSender.SendNotificationAsync(log);
                        log.Status = NotificationStatus.Sent;
                        _logger.LogInformation("Retry başarılı: {NotificationId}", log.Id);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Retry başarısız: {NotificationId}", log.Id);
                    }
                }

                await unitOfWork.SaveChangesAsync();
            }
        }
    }
}