using MediatR;
using OrderService.Application.Abstractions.Messaging;
using OrderService.Application.DTOs;
using OrderService.Domain.Events;

namespace OrderService.Application.Orders.Events
{
    public class OrderCreatedIntegrationEventHandler : INotificationHandler<OrderCreatedEvent>
    {
        private readonly IEventBusPublisher _eventBusPublisher;

        public OrderCreatedIntegrationEventHandler(IEventBusPublisher eventBusPublisher)
        {
            _eventBusPublisher = eventBusPublisher;
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

            await _eventBusPublisher.PublishAsync(integrationEvent, queueName: "order-created");
        }
    }
}
