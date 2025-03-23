using MediatR;
using StockService.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockService.Domain.Events
{
    public class StockDecreasedEvent : DomainEvent
    {
        public int ProductId { get; }
        public int NewQuantity { get; }

        public StockDecreasedEvent(int productId, int newQuantity)
        {
            ProductId = productId;
            NewQuantity = newQuantity;
        }
    }
}
