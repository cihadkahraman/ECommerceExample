using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OrderService.Application.Common.Serialization;
using OrderService.Application.DTOs;
using OrderService.Application.Orders.Commands;
using OrderService.Application.Orders.Queries;
using OrderService.Application.Common.Logging;
using Serilog.Context;
using System.Text.Json;
using AutoMapper;
using OrderService.Api.Models;
using OrderService.Domain.Enums;

namespace OrderService.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<OrderController> _logger;
        private readonly IMapper _mapper;

        public OrderController(IMediator mediator, ILogger<OrderController> logger, IMapper mapper)
        {
            _mediator = mediator;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpPost("create")]
        public async Task<ActionResult<CreatedOrderDto>> CreateOrder([FromBody] CreateOrderRequest request)
        {
            var response = new CreateOrderResponse();
            var command = _mapper.Map<CreateOrderCommand>(request);
            var correlationId = HttpContext.Items["CorrelationId"]?.ToString() ?? System.Guid.NewGuid().ToString();
            _logger.LogInformationWithPayload($"{command.CustomerId} numaralı müşteri için sipariş oluşturuluyor.", correlationId);
            var commandResult = await _mediator.Send(command);
            response.OrderId = commandResult.OrderId;
            response.OrderStatus= OrderStatus.Pending.ToString();
            response.Message = "Siparişiniz alınmıştır. Siparişinizi ilgili bölümden takip edebilirsiniz.";
            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<OrderDto>> GetOrderById(Guid id)
        {
            var result = await _mediator.Send(new GetOrderByIdQuery(id));
            if (result == null)
                return NotFound();

            return Ok(result);
        }

        [HttpGet("customer/{customerId}")]
        public async Task<ActionResult<List<OrderDto>>> GetOrdersByCustomerId(int customerId)
        {
            var result = await _mediator.Send(new GetOrdersByCustomerIdQuery(customerId));
            return Ok(result);
        }
    }
}
