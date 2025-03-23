using StockService.Domain.Common;
using StockService.Domain.Events;
using StockService.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockService.Domain.Entities
{
    public class Stock : AggregateRoot
    {
        public int ProductId { get; private set; }
        public int Quantity { get; private set; }
        public Product Product { get; private set; }

        private Stock() { }

        public Stock(int productId, string sku, int quantity)
        {
            ProductId = productId;
            Quantity = quantity;
        }

        public void Decrease(int amount)
        {
            if (amount <= 0)
                throw new DomainException("Azaltmak istediğiniz stok adedi pozitif olmalı!");

            if (Quantity < amount)
                throw new DomainException("Stoğu olmayan ürünü azaltamazsınız!");

            Quantity -= amount;
            AddDomainEvent(new StockDecreasedEvent(ProductId, amount));
        }

        public void Increase(int amount)
        {
            if (amount <= 0)
                throw new DomainException("Arttırmak istediğiniz stok adedi pozitif olmalı!");

            Quantity += amount;
            AddDomainEvent(new StockIncreasedEvent(ProductId, amount));
        }
    }
}
