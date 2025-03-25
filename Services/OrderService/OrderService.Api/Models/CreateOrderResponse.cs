using OrderService.Domain.Enums;
using System.Text.Json.Serialization;

namespace OrderService.Api.Models
{
    public class CreateOrderResponse
    {
        [JsonPropertyName("Order Number")]
        public int OrderId { get; set; }
        [JsonPropertyName("Order Status")]
        public string OrderStatus { get; set; }
        public string Message { get; set; }
    }
}
