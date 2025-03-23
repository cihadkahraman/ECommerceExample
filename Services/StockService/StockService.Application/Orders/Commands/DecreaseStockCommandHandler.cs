using MediatR;
using StockService.Application.Abstractions;
using StockService.Application.Abstractions.Messaging;
using StockService.Application.Orders.Events;
using StockService.Application.Orders.Events.Outgoing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockService.Application.Orders.Commands
{
    public class DecreaseStockCommandHandler : IRequestHandler<DecreaseStockCommand, bool>
    {
        private readonly IStockRepository _stockRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEventBusPublisher _eventBusPublisher;

        public DecreaseStockCommandHandler(
            IStockRepository stockRepository,
            IUnitOfWork unitOfWork,
            IEventBusPublisher eventBusPublisher)
        {
            _stockRepository = stockRepository;
            _unitOfWork = unitOfWork;
            _eventBusPublisher = eventBusPublisher;
        }

        public async Task<bool> Handle(DecreaseStockCommand request, CancellationToken cancellationToken)
        {
            var stock = await _stockRepository.GetByProductIdAsync(request.ProductId);

            if (stock == null)
                throw new InvalidOperationException("Stok bulunamadı");

            stock.Decrease(request.Quantity);

            var integrationEvent = new StockDecreasedIntegrationEvent
            {
                ProductId = stock.ProductId,
                NewQuantity = stock.Quantity
            };

            await _eventBusPublisher.PublishAsync(integrationEvent, queueName: "stock-decreased");

            await _unitOfWork.SaveChangesAsync();

            return true;
        }
    }
}
