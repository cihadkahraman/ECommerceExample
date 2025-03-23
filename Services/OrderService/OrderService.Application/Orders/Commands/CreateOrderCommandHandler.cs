using MediatR;
using OrderService.Application.Abstractions;
using OrderService.Application.Abstractions.Messaging;
using OrderService.Application.DTOs;
using OrderService.Domain.Entities;
using OrderService.Domain.ValueObjects;

namespace OrderService.Application.Orders.Commands
{
    public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, CreatedOrderDto>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEventBusPublisher _eventBusPublisher;

        public CreateOrderCommandHandler(IOrderRepository orderRepository, IUnitOfWork unitOfWork, IEventBusPublisher eventBusPublisher)
        {
            _orderRepository = orderRepository;
            _unitOfWork = unitOfWork;
            _eventBusPublisher = eventBusPublisher;
        }

        public async Task<CreatedOrderDto> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            var address = new Address(
                request.Street,
                request.City,
                request.State,
                request.ZipCode,
                request.Country
            );
            
            List<OrderItem> orderItems = new List<OrderItem>();

            foreach (var item in request.Items)
            {
                var orderItem = new OrderItem(
                    item.ProductId,
                    item.ProductName,
                    item.Quantity,
                    new Money(item.Price)
                );

                orderItems.Add(orderItem);
            }

            var order = new Order(request.CustomerId, address, orderItems);

            await _orderRepository.AddAsync(order);

            await _unitOfWork.SaveChangesAsync();

            return new CreatedOrderDto
            {
                OrderId = order.Id
            };
        }
    }
}
