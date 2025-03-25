using Microsoft.Extensions.DependencyInjection;
using OrderService.Application.Abstractions;
using OrderService.Infrastructure.Persistence;
using OrderService.Infrastructure.Persistence.Contexts;
using OrderService.Infrastructure.Persistence.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using OrderService.Infrastructure.Services;
using MassTransit;
using OrderService.Application.Abstractions.Messaging;
using OrderService.Infrastructure.Messaging;
using OrderService.Application.Orders.Events;
using RabbitMQ.Client;
using Microsoft.Extensions.Logging;
using OrderService.Application.Orders.Consumers;

namespace OrderService.Infrastructure
{
    public static class InfrastructureServiceRegistration
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<OrderDbContext>(options =>
            {
                options.UseNpgsql(configuration.GetConnectionString("OrderConnection"));
            });

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IDomainEventDispatcher, DomainEventDispatcher>();
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

            services.AddMassTransit(x =>
            {
                x.SetKebabCaseEndpointNameFormatter();

                x.AddConsumer<StockNotReservedIntegrationEventConsumer>();
                x.AddConsumer<StockReservedIntegrationEventConsumer>();

                x.AddEntityFrameworkOutbox<OrderDbContext>(o =>
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
                    cfg.Message<OrderCreatedIntegrationEvent>(x =>
                    {
                        x.SetEntityName("order.created");
                    });

                    cfg.Publish<OrderCreatedIntegrationEvent>(p =>
                    {
                        p.ExchangeType = ExchangeType.Fanout;
                    });

                    cfg.ReceiveEndpoint("order-stock-notreserved-queue", e =>
                    {
                        e.ConfigureConsumer<StockNotReservedIntegrationEventConsumer>(context);

                        e.Bind("stock.notreserved", b => b.ExchangeType = ExchangeType.Fanout);
                    });

                    cfg.ReceiveEndpoint("order-stock-reserved-queue", e =>
                    {
                        e.ConfigureConsumer<StockReservedIntegrationEventConsumer>(context);

                        e.Bind("stock.reserved", b => b.ExchangeType = ExchangeType.Fanout);
                    });

                    cfg.ConfigureEndpoints(context);
                });

            });

            services.AddScoped<IEventBusPublisher, MassTransitEventBusPublisher>();

            return services;
        }
    }
}
