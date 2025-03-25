using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockService.Application.Orders.Events.Incoming
{
    [MessageUrn("order.created")]
    public class OrderCreatedIntegrationEvent
    {
        //public Guid CorrelationId { get; set; }
        public Guid OrderId { get; set; }
        public int CustomerId { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<OrderItemDto> Items { get; set; } = new();
    }
    public class OrderItemDto
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}
