using OrderService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderService.Application.Abstractions
{
    public interface IOrderRepository : IRepository<Order>
    {
        Task<List<Order>> GetOrdersByCustomerIdAsync(int customerId);
    }
}
