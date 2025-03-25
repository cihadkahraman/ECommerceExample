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

namespace OrderService.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<OrderController> _logger;

        public OrderController(IMediator mediator, ILogger<OrderController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpPost("create")]
        public async Task<ActionResult<CreatedOrderDto>> CreateOrder(CreateOrderCommand command)
        {

            _logger.LogInformationWithPayload($"{command.CustomerId} numaralı müşteri için sipariş oluşturuluyor.");
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<OrderDto>> GetOrderById(int id)
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
