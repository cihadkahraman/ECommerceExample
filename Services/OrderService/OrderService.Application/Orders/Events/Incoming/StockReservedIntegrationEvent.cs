using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderService.Application.Orders.Events.Incoming
{
    [MessageUrn("stock.reserved")]
    public class StockReservedIntegrationEvent
    {
        public Guid OrderId { get; set; }
        public int CustomerId { get; set; }
        public DateTime ReservedAt { get; set; }
        public List<OrderItemDto> Items { get; set; }

        public StockReservedIntegrationEvent(Guid orderId, int customerId, DateTime reservedAt, List<OrderItemDto> items)
        {
            OrderId = orderId;
            CustomerId = customerId;
            ReservedAt = reservedAt;
            Items = items;
        }
    }
}
