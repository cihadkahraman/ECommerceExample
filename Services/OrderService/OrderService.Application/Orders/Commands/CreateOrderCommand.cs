using MediatR;
using OrderService.Application.DTOs;

namespace OrderService.Application.Orders.Commands
{
    public class CreateOrderCommand : IRequest<CreatedOrderDto>
    {
        public int CustomerId { get; set; }
        public string Street { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string State { get; set; } = string.Empty;
        public string ZipCode { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;

        public List<CreateOrderItem> Items { get; set; } = new();
    }

    public class CreateOrderItem
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}
