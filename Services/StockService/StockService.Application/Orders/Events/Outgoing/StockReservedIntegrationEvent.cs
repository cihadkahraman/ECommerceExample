using StockService.Application.Orders.Events.Incoming;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockService.Application.Orders.Events.Outgoing
{
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
