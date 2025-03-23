using AutoMapper;
using MediatR;
using OrderService.Application.Abstractions;
using OrderService.Application.DTOs;
using OrderService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderService.Application.Orders.Queries
{
    public class GetOrdersByCustomerIdQueryHandler : IRequestHandler<GetOrdersByCustomerIdQuery, List<OrderDto>>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;

        public GetOrdersByCustomerIdQueryHandler(IOrderRepository orderRepository, IMapper mapper)
        {
            _orderRepository = orderRepository;
            _mapper = mapper;
        }

        public async Task<List<OrderDto>> Handle(GetOrdersByCustomerIdQuery request, CancellationToken cancellationToken)
        {
            var orders = await _orderRepository.GetOrdersByCustomerIdAsync(request.CustomerId);
            return _mapper.Map<List<OrderDto>>(orders);
        }
    }
}
