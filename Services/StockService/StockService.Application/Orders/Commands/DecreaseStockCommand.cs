using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockService.Application.Orders.Commands
{
    public class DecreaseStockCommand : IRequest<bool>
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }

        public DecreaseStockCommand(int productId, int quantity)
        {
            ProductId = productId;
            Quantity = quantity;
        }
    }
}
