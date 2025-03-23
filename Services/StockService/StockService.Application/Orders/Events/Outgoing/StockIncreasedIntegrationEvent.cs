using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockService.Application.Orders.Events.Outgoing
{
    public class StockIncreasedIntegrationEvent
    {
        public int ProductId { get; set; }
        public int NewQuantity { get; set; }
        public DateTime IncreasedAt { get; set; } = DateTime.UtcNow;
    }
}
