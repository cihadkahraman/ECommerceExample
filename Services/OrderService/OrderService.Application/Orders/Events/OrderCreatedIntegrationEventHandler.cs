using MediatR;
using Microsoft.Extensions.Logging;
using OrderService.Application.Abstractions.Messaging;
using OrderService.Domain.Events;

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
                CorrelationId = correlationId,
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

            await _eventBusPublisher.PublishAsync(integrationEvent);
        }
    }
}
