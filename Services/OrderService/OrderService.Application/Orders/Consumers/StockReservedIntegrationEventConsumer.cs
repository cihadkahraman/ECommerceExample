using MassTransit;
using Microsoft.Extensions.Logging;
using OrderService.Application.Abstractions;
using OrderService.Application.Common.Logging;
using OrderService.Application.Common.Models;
using OrderService.Application.Orders.Events.Incoming;
using OrderService.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderService.Application.Orders.Consumers
{
    [MessageUrn("stock.reserved")]
    public class StockReservedIntegrationEventConsumer : IConsumer<StockReservedIntegrationEvent>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<StockReservedIntegrationEventConsumer> _logger;

        public StockReservedIntegrationEventConsumer(
            IOrderRepository orderRepository,
            IUnitOfWork unitOfWork,
            ILogger<StockReservedIntegrationEventConsumer> logger)
        {
            _orderRepository = orderRepository;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<StockReservedIntegrationEvent> context)
        {
            var message = context.Message;
            var correlationId = new CorrelationId(context.Headers.GetCorrelationId().Value);

            _logger.LogInformationWithPayload($"StockReservedIntegrationEvent alındı. OrderId: {message.OrderId}", correlationId);

            var order = await _orderRepository.GetByIdAsync(message.OrderId);
            if (order == null)
            {
                _logger.LogError("Order bulunamadı: {OrderId}", message.OrderId);
                throw new DomainException(message.OrderId.ToString());
            }

            order.MarkAsCompleted();
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformationWithPayload($"Order status 'Completed' olarak güncellendi. OrderId: {message.OrderId}", correlationId, order);
        }
    }
}
