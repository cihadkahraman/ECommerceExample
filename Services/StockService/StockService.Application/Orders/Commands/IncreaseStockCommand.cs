using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockService.Application.Orders.Commands
{
    public class IncreaseStockCommand : IRequest<bool>
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }

        public IncreaseStockCommand(int productId, int quantity)
        {
            ProductId = productId;
            Quantity = quantity;
        }
    }
}
