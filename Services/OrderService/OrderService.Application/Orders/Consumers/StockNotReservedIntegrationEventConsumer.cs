using MassTransit;
using Microsoft.Extensions.Logging;
using OrderService.Application.Abstractions;
using OrderService.Domain.ValueObjects;
using OrderService.Domain.Exceptions;
using OrderService.Application.Orders.Events.Outgoing;
using OrderService.Domain.Enums;
using OrderService.Application.Common.Logging;
using OrderService.Application.Common.Models;

namespace OrderService.Application.Orders.Consumers
{
    [MessageUrn("stock.notreserved")]
    public class StockNotReservedIntegrationEventConsumer : IConsumer<StockNotReservedIntegrationEvent>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<StockNotReservedIntegrationEventConsumer> _logger;

        public StockNotReservedIntegrationEventConsumer(
            IOrderRepository orderRepository,
            IUnitOfWork unitOfWork,
            ILogger<StockNotReservedIntegrationEventConsumer> logger)
        {
            _orderRepository = orderRepository;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<StockNotReservedIntegrationEvent> context)
        {
            var message = context.Message;
            var correlationId = new CorrelationId(context.Headers.GetCorrelationId().Value);

            _logger.LogInformation("StockNotReservedIntegrationEvent alındı. OrderId: {OrderId}, CorrelationId: {CorrelationId}",
                message.OrderId, correlationId);

            var order = await _orderRepository.GetByIdAsync(message.OrderId);
            if (order == null)
            {
                _logger.LogError("Order bulunamadı: {OrderId}", message.OrderId);
                throw new DomainException(message.OrderId.ToString());
            }

            order.MarkAsStockFailed();
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformationWithPayload($"Order status 'StockFailed' olarak güncellendi. OrderId: {message.OrderId}", correlationId, order);
        }
    }
}
