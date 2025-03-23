using StockService.Domain.Common;
using StockService.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockService.Domain.Entities
{
    public class Product : Entity
    {
        public string Name { get; private set; }
        public string Description { get; private set; }

        public Money Price { get; private set; }

        public Stock Stock { get; private set; }

        private Product() { }

        public Product(string name, string description, Money price)
        {
            Name = name;
            Description = description;
            Price = price;
        }
    }
}
