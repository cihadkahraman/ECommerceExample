using MediatR;
using OrderService.Application.DTOs;
using OrderService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderService.Application.Orders.Queries
{
    public class GetOrdersByCustomerIdQuery : IRequest<List<OrderDto>>
    {
        public int CustomerId { get; set; }

        public GetOrdersByCustomerIdQuery(int customerId)
        {
            CustomerId = customerId;
        }
    }
}
