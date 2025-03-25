using OrderService.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderService.Application.DTOs
{
    public class CreateOrderRequest
    {
        public int CustomerId { get; set; }
        public Address Address { get; set; }
        public List<CreateOrderItemDto> Items { get; set; }
    }
}
