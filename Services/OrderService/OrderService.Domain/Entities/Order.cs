﻿using OrderService.Domain.Common;
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
        public long OrderNumber { get; private set; }
        public int CustomerId { get; private set; }
        public Address ShippingAddress { get; private set; }
        public OrderStatus Status { get; private set; }

        private readonly List<OrderItem> _items = new();
        public IReadOnlyCollection<OrderItem> Items => _items.AsReadOnly();

        private Order() { }

        public Order(int customerId, Address shippingAddress, List<OrderItem> items)
        {
            if (!items.Any())
                throw new DomainException("Siparişte en az bir ürün olmalıdır.");

            CustomerId = customerId;
            ShippingAddress = shippingAddress;
            Status = OrderStatus.Pending;

            ;

            foreach (var item in items)
            {
                AddItem(item);
            }

            AddDomainEvent(new OrderCreatedEvent(Id, CustomerId, CreatedAt, _items));
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

        public void MarkAsStockFailed()
        {
            if (Status != OrderStatus.Pending)
                throw new DomainException("Yalnızca bekleyen siparişler iptal edildi olarak işaretlenebilir.");

            Status = OrderStatus.Cancelled;
        }

        public void MarkAsCompleted()
        {
            if (Status != OrderStatus.Pending)
                throw new DomainException("Yalnızca bekleyen siparişler tamamlandı olarak işaretlenebilir.");

            Status = OrderStatus.Completed;
        }

    }
}
