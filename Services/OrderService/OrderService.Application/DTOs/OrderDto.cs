using OrderService.Application.Orders.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderService.Application.DTOs
{
    public class OrderDto
    {
        public Guid OrderId { get; set; }
        public int CustomerId { get; set; }

        public string Street { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string State { get; set; } = string.Empty;
        public string ZipCode { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;

        public string Status { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }

        public List<OrderItemDto> Items { get; set; } = new();
    }

    
}
