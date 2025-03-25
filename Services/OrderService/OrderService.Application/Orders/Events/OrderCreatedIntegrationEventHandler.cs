using MediatR;
using Microsoft.Extensions.Logging;
using OrderService.Application.Abstractions.Messaging;
using OrderService.Application.Common.Serialization;
using OrderService.Domain.Events;
using System.Text.Json;

namespace OrderService.Application.Orders.Events
{
    public class OrderCreatedIntegrationEventHandler : INotificationHandler<OrderCreatedEvent>
    {
        private readonly IEventBusPublisher _eventBusPublisher;
        private readonly ILogger<OrderCreatedIntegrationEventHandler> _logger;

        public OrderCreatedIntegrationEventHandler(IEventBusPublisher eventBusPublisher, ILogger<OrderCreatedIntegrationEventHandler> logger)
        {
            _eventBusPublisher = eventBusPublisher;
            _logger = logger;
        }

        public async Task Handle(OrderCreatedEvent notification, CancellationToken cancellationToken)
        {
            var correlationId = Guid.NewGuid();
            var integrationEvent = new OrderCreatedIntegrationEvent
            {
                OrderId = notification.OrderId,
                CustomerId = notification.CustomerId,
                CreatedAt = DateTime.UtcNow
            };

            foreach (var item in notification.OrderItems)
            {
                integrationEvent.Items.Add(new OrderItemDto
                {
                    ProductId = item.ProductId,
                    ProductName = item.ProductName,
                    Quantity = item.Quantity,
                    Price = item.Price.Amount
                });
            }
            _logger.LogInformation("Order created: {@Event}", integrationEvent);

            var json = JsonSerializer.Serialize(integrationEvent, JsonDefaults.Options);

            await _eventBusPublisher.PublishAsync(integrationEvent);
        }
    }
}
