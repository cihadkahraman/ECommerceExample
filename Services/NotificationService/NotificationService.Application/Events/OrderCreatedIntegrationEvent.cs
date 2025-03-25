using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace NotificationService.Application.Events
{
    [MessageUrn("order.created")]
    public class OrderCreatedIntegrationEvent
    {
        [JsonPropertyName("orderId")]
        public Guid OrderId { get; set; }
        [JsonPropertyName("customerId")]
        public int CustomerId { get; set; }
        [JsonPropertyName("createdAt")]
        public DateTime CreatedAt { get; set; }
        [JsonPropertyName("items")]
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
