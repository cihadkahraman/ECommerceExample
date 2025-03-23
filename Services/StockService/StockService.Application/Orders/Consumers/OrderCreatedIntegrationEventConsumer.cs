using MassTransit;
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

        public OrderCreatedIntegrationEventConsumer(IStockRepository stockRepository, IUnitOfWork unitOfWork)
        {
            _stockRepository = stockRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task Consume(ConsumeContext<OrderCreatedIntegrationEvent> context)
        {
            var message = context.Message;

            Console.WriteLine($"[StockService] Order received: {message.OrderId} - Customer: {message.CustomerId}");

            foreach (var item in message.Items)
            {
                var stock = await _stockRepository.GetByProductIdAsync(item.ProductId);

                if (stock == null)
                    throw new InvalidOperationException("Stok bulunamadı");

                stock.Decrease(item.Quantity);
            }

            await _unitOfWork.SaveChangesAsync();
        }
    }
}
