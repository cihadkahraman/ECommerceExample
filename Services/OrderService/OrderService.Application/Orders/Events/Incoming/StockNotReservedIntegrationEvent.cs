using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderService.Application.Orders.Events.Incoming
{
    [MessageUrn("stock.notreserved")]
    public class StockNotReservedIntegrationEvent
    {
        public Guid OrderId { get; set; }
        public int CustomerId { get; set; }
        public string Reason { get; set; }
        public DateTime FailedAt { get; set; }

        public StockNotReservedIntegrationEvent(Guid orderId, int customerId, string reason, DateTime failedAt)
        {
            OrderId = orderId;
            CustomerId = customerId;
            Reason = reason;
            FailedAt = failedAt;
        }
    }
}
