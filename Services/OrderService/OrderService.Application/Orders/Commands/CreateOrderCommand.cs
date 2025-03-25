using MediatR;
using OrderService.Application.DTOs;
using OrderService.Domain.ValueObjects;

namespace OrderService.Application.Orders.Commands
{
    public class CreateOrderCommand : IRequest<CreatedOrderDto>
    {
        public int CustomerId { get; set; }
        public Address Address { get; set; }
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
