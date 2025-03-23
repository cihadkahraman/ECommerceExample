using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderService.Domain.Events
{
    public class OrderCreatedEvent : INotification
    {
        public int OrderId { get; }
        public int CustomerId { get; }
        public DateTime CreatedAt { get; }

        public OrderCreatedEvent(int orderId, int customerId, DateTime createdAt)
        {
            OrderId = orderId;
            CustomerId = customerId;
            CreatedAt = createdAt;
        }
    }
}
