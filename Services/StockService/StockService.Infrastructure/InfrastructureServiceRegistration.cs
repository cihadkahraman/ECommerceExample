using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StockService.Infrastructure.Persistence.Repositories;
using StockService.Infrastructure.Persistence;
using StockService.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using StockService.Application.Abstractions;
using MassTransit;
using StockService.Application.Abstractions.Messaging;
using StockService.Infrastructure.Messaging;
using StockService.Application.Orders.Consumers;
using RabbitMQ.Client;
using StockService.Application.Orders.Events.Incoming;
using StockService.Application.Orders.Events.Outgoing;

namespace StockService.Infrastructure
{
    public static class InfrastructureServiceRegistration
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<StockDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("StockConnection")));

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IStockRepository, StockRepository>();
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

            services.AddMassTransit(x =>
            {
                x.SetKebabCaseEndpointNameFormatter();

                x.AddConsumer<OrderCreatedIntegrationEventConsumer>();

                x.AddEntityFrameworkOutbox<StockDbContext>(o =>
                {
                    o.UsePostgres();
                    o.UseBusOutbox();
                    o.QueryDelay = TimeSpan.FromSeconds(1);
                });

                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host(configuration["RabbitMq:Host"], "/", h =>
                    {
                        h.Username(configuration["RabbitMq:Username"]);
                        h.Password(configuration["RabbitMq:Password"]);
                    });

                    cfg.Message<StockNotReservedIntegrationEvent>(x =>
                    {
                        x.SetEntityName("stock.notreserved");
                    });

                    cfg.Message<StockReservedIntegrationEvent>(x =>
                    {
                        x.SetEntityName("stock.reserved");
                    });

                    cfg.ReceiveEndpoint("stock-order-created-queue", e =>
                    {
                        e.ConfigureConsumer<OrderCreatedIntegrationEventConsumer>(context);

                        e.Bind("order.created", b => b.ExchangeType = ExchangeType.Fanout);
                    });
                });
            });

            services.AddScoped<IEventBusPublisher, MassTransitEventBusPublisher>();

            return services;
        }
    }
}
