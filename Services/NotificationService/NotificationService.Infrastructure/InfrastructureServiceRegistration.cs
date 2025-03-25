using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NotificationService.Infrastructure.Persistence.Contexts;
using NotificationService.Infrastructure.Persistence.Repositories;
using NotificationService.Infrastructure.Persistence;
using MassTransit;
using NotificationService.Application.Abstractions;
using NotificationService.Application.Consumers;
using Microsoft.EntityFrameworkCore;
using RabbitMQ.Client;
using NotificationService.Infrastructure.Configuration;
using NotificationService.Application.Services;
using NotificationService.Infrastructure.Services;
using NotificationService.Infrastructure.BackgroundJobs;

namespace NotificationService.Infrastructure
{
    public static class InfrastructureServiceRegistration
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<NotificationDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("NotificationConnection")));

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<INotificationLogRepository, NotificationLogRepository>();
            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.Configure<SmtpSettings>(configuration.GetSection("SmtpSettings"));
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<ISmsService, FakeSmsService>();
            services.AddHostedService<FailedNotificationRetryJob>();



            services.AddMassTransit(x =>
            {
                x.SetKebabCaseEndpointNameFormatter();

                x.AddConsumer<OrderCreatedIntegrationEventConsumer>();

                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host(configuration["RabbitMq:Host"], "/", h =>
                    {
                        h.Username(configuration["RabbitMq:Username"]);
                        h.Password(configuration["RabbitMq:Password"]);
                    });

                    cfg.ReceiveEndpoint("notification-order-created-queue", e =>
                    {
                        e.ConfigureConsumer<OrderCreatedIntegrationEventConsumer>(context);

                        e.Bind("order.created", b => b.ExchangeType = ExchangeType.Fanout);
                    });
                });
            });

            return services;
        }
    }
}

