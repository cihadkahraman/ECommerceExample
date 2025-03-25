using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NotificationService.Application.Services;
using NotificationService.Infrastructure.Configuration;
using Polly;
using Polly.Retry;

namespace NotificationService.Infrastructure.Services
{
    public class EmailService : IEmailService
    {
        private readonly SmtpSettings _smtpSettings;
        private readonly ILogger<EmailService> _logger;
        private readonly AsyncRetryPolicy _retryPolicy;

        public EmailService(IOptions<SmtpSettings> smtpSettings, ILogger<EmailService> logger)
        {
            _smtpSettings = smtpSettings.Value;
            _logger = logger;

            _retryPolicy = Policy
            .Handle<SmtpException>()
            .Or<TimeoutException>()
            .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(2),
                (exception, timeSpan, retryCount, context) =>
                {
                    _logger.LogWarning("Retry {RetryCount} after {Delay}. Hata: {Message}", retryCount, timeSpan, exception.Message);
                });
        }

        public async Task SendEmailAsync(string to, string subject, string body)
        {
            await _retryPolicy.ExecuteAsync(async () =>
            {
                try
                {
                    var mail = new MailMessage();
                    mail.From = new MailAddress(_smtpSettings.From);
                    mail.To.Add(to);
                    mail.Subject = subject;
                    mail.Body = body;
                    mail.IsBodyHtml = false;
                    using var smtpClient = new SmtpClient(_smtpSettings.Host, _smtpSettings.Port)
                    {
                        Credentials = new NetworkCredential(_smtpSettings.Username, _smtpSettings.Password),
                        EnableSsl = _smtpSettings.EnableSsl
                    };
                    await smtpClient.SendMailAsync(mail);
                    _logger.LogInformation("Email gönderildi: {To}", to);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Email gönderimi başarısız: {To}", to);
                    throw;
                }
            });
        }
    }
}
