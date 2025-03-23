using MediatR;
using Microsoft.AspNetCore.Mvc;
using StockService.Application.Orders.Commands;

namespace StockService.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StockController : ControllerBase
    {
        private readonly IMediator _mediator;

        public StockController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("decrease")]
        public async Task<IActionResult> DecreaseStock([FromBody] DecreaseStockCommand command)
        {
            var result = await _mediator.Send(command);
            if (!result)
                return BadRequest("Stok azaltılamadı veya ürün bulunamadı.");
            return Ok("Stok başarıyla azaltıldı.");
        }

        [HttpPost("increase")]
        public async Task<IActionResult> IncreaseStock([FromBody] IncreaseStockCommand command)
        {
            var result = await _mediator.Send(command);
            if (!result)
                return BadRequest("Stok artırılamadı veya ürün bulunamadı.");
            return Ok("Stok başarıyla artırıldı.");
        }
    }
}