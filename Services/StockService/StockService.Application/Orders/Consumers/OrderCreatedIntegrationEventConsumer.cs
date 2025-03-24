using MassTransit;
using Microsoft.Extensions.Logging;
using StockService.Application.Abstractions;
using StockService.Application.Orders.Events.Incoming;
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

        public OrderCreatedIntegrationEventConsumer(IStockRepository stockRepository, IUnitOfWork unitOfWork, ILogger<OrderCreatedIntegrationEventConsumer> logger)
        {
            _stockRepository = stockRepository;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<OrderCreatedIntegrationEvent> context)
        {
            var message = context.Message;

            foreach (var item in message.Items)
            {
                var stock = await _stockRepository.GetByProductIdAsync(item.ProductId);

                if (stock == null)
                {
                    _logger.LogError("Stok bulunamadı {ProductId}, {CorrelationId}", item.ProductId, context.CorrelationId);
                    throw new InvalidOperationException("Stok bulunamadı");
                }
                
                _logger.LogInformation("Stok düşürülüyor {ProductId} - {Quantity} : {CorrelationId}", item.ProductId, item.Quantity, context.CorrelationId);

                stock.Decrease(item.Quantity);
            }

            await _unitOfWork.SaveChangesAsync();
        }
    }
}
