using OrderService.Domain.Common;
using OrderService.Domain.Enums;
using OrderService.Domain.Events;
using OrderService.Domain.Exceptions;
using OrderService.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderService.Domain.Entities
{
    public class Order : AggregateRoot
    {
        public int CustomerId { get; private set; }
        public Address ShippingAddress { get; private set; }
        public OrderStatus Status { get; private set; }
        public DateTime CreatedAt { get; private set; }

        private readonly List<OrderItem> _items = new();
        public IReadOnlyCollection<OrderItem> Items => _items.AsReadOnly();

        private Order() { }

        public Order(int customerId, Address shippingAddress)
        {
            CustomerId = customerId;
            ShippingAddress = shippingAddress;
            CreatedAt = DateTime.UtcNow;
            Status = OrderStatus.Pending;

            AddDomainEvent(new OrderCreatedEvent(Id, CustomerId, CreatedAt));
        }

        public void AddItem(OrderItem item)
        {
            if (Status != OrderStatus.Pending)
                throw new DomainException("Sipariş durumu ürün eklemeye uygun değil.");

            _items.Add(item);
        }

        public void Complete()
        {
            if (!_items.Any())
                throw new DomainException("Sipariş boş olamaz.");

            Status = OrderStatus.Completed;
        }

    }
}
