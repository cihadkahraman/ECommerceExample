using OrderService.Domain.Common;
using OrderService.Domain.Entities;

namespace OrderService.Domain.Events
{
    public class OrderCreatedEvent : DomainEvent
    {
        public Guid OrderId { get; }
        public int CustomerId { get; }
        public List<OrderItem> OrderItems { get; } = new();

        public OrderCreatedEvent(Guid orderId, int customerId, DateTime createdAt, List<OrderItem> orderItems)
        {
            OrderId = orderId;
            CustomerId = customerId;
            foreach (var item in orderItems)
            {
                OrderItems.Add(item);
            }
        }
    }
}
