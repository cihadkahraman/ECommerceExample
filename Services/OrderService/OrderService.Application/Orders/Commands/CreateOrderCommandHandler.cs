using MediatR;
using OrderService.Application.Abstractions;
using OrderService.Application.Abstractions.Messaging;
using OrderService.Application.DTOs;
using OrderService.Application.Orders.Events;
using OrderService.Domain.Entities;
using OrderService.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

            var order = new Order(request.CustomerId, address);

            foreach (var item in request.Items)
            {
                var orderItem = new OrderItem(
                    item.ProductId,
                    item.ProductName,
                    item.Quantity,
                    new Money(item.Price)
                );

                order.AddItem(orderItem);
            }

            await _orderRepository.AddAsync(order);

            var correlationId = Guid.NewGuid();
            var integrationEvent = new OrderCreatedIntegrationEvent
            {
                CorrelationId = correlationId,
                OrderId = order.Id,
                CustomerId = order.CustomerId,
                CreatedAt = DateTime.UtcNow
            };

            await _eventBusPublisher.PublishAsync(integrationEvent, queueName: "order-created");

            await _unitOfWork.SaveChangesAsync();

            return new CreatedOrderDto
            {
                OrderId = order.Id
            };
        }
    }
}
