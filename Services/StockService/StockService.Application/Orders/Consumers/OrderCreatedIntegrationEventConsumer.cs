using MassTransit;
using Microsoft.Extensions.Logging;
using StockService.Application.Abstractions;
using StockService.Application.Abstractions.Messaging;
using StockService.Application.Common.Logging;
using StockService.Application.Common.Models;
using StockService.Application.Orders.Events.Incoming;
using StockService.Application.Orders.Events.Outgoing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockService.Application.Orders.Consumers
{
    public class OrderCreatedIntegrationEventConsumer : IConsumer<OrderCreatedIntegrationEvent>
    {
        private readonly IStockRepository _stockRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<OrderCreatedIntegrationEventConsumer> _logger;
        private readonly IEventBusPublisher _eventBusPublisher;

        public OrderCreatedIntegrationEventConsumer(
        IStockRepository stockRepository,
        IUnitOfWork unitOfWork,
        IEventBusPublisher eventBusPublisher,
        ILogger<OrderCreatedIntegrationEventConsumer> logger)
        {
            _stockRepository = stockRepository;
            _unitOfWork = unitOfWork;
            _eventBusPublisher = eventBusPublisher;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<OrderCreatedIntegrationEvent> context)
        {
            var message = context.Message;
            var correlationId = new CorrelationId(context.Headers.GetCorrelationId().Value);

            bool allInStock = true;

            foreach (var item in message.Items)
            {
                var stock = await _stockRepository.GetByProductIdAsync(item.ProductId);

                if (stock == null || stock.Quantity < item.Quantity)
                {
                    _logger.LogErrorWithPayload(null,$"Yetersiz stok veya stok bulunamadı: {item.ProductId}",correlationId);

                    allInStock = false;
                    break;
                }
            }

            if (allInStock)
            {
                foreach (var item in message.Items)
                {
                    var stock = await _stockRepository.GetByProductIdAsync(item.ProductId);
                    stock!.Decrease(item.Quantity);
                }

                await _unitOfWork.SaveChangesAsync();

                var stockReservedEvent = new StockReservedIntegrationEvent(
                    message.OrderId,
                    message.CustomerId,
                    DateTime.UtcNow,
                    message.Items
                );

                await _eventBusPublisher.PublishAsync(stockReservedEvent);

                _logger.LogInformationWithPayload($"StockReservedIntegrationEvent gönderildi. OrderId: {message.OrderId}", correlationId);
            }
            else
            {
                var notReservedEvent = new StockNotReservedIntegrationEvent(
                    message.OrderId,
                    message.CustomerId,
                    "Yetersiz stok",
                    DateTime.UtcNow
                );

                await _eventBusPublisher.PublishAsync(notReservedEvent);

                _logger.LogInformationWithPayload($"StockNotReservedIntegrationEvent gönderildi. OrderId: OrderId: {message.OrderId}", correlationId);
            }
        }
    }
}
