using MediatR;
using StockService.Application.Abstractions.Messaging;
using StockService.Application.Abstractions;
using StockService.Application.Orders.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StockService.Application.Orders.Events.Outgoing;

namespace StockService.Application.Orders.Commands
{
    public class IncreaseStockCommandHandler : IRequestHandler<IncreaseStockCommand, bool>
    {
        private readonly IStockRepository _stockRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEventBusPublisher _eventBusPublisher;

        public IncreaseStockCommandHandler(
            IStockRepository stockRepository,
            IUnitOfWork unitOfWork,
            IEventBusPublisher eventBusPublisher)
        {
            _stockRepository = stockRepository;
            _unitOfWork = unitOfWork;
            _eventBusPublisher = eventBusPublisher;
        }

        public async Task<bool> Handle(IncreaseStockCommand request, CancellationToken cancellationToken)
        {
            var stock = await _stockRepository.GetByProductIdAsync(request.ProductId);

            if (stock == null)
                throw new InvalidOperationException("Stok bulunamadı");

            stock.Increase(request.Quantity);

            var integrationEvent = new StockIncreasedIntegrationEvent
            {
                ProductId = stock.ProductId,
                NewQuantity = stock.Quantity
            };

            await _eventBusPublisher.PublishAsync(integrationEvent, queueName: "stock-increased");

            await _unitOfWork.SaveChangesAsync();

            return true;
        }
    }
}
