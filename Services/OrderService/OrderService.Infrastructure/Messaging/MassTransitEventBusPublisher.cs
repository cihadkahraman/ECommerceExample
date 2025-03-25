using MassTransit;
using Microsoft.AspNetCore.Http;
using OrderService.Application.Abstractions.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace OrderService.Infrastructure.Messaging
{
    public class MassTransitEventBusPublisher : IEventBusPublisher
    {
        private readonly IBus _bus;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public MassTransitEventBusPublisher(IBus bus, IHttpContextAccessor httpContextAccessor)
        {
            _bus = bus;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task PublishAsync<T>(T message) where T : class
        {
            var a = _httpContextAccessor.HttpContext?.Items["CorrelationId"];
            var correlationId =
            _httpContextAccessor.HttpContext?.Items["CorrelationId"]?.ToString() ??
            Guid.NewGuid().ToString();

            await _bus.Publish(message, publishContext =>
            {
                publishContext.CorrelationId = Guid.TryParse(correlationId, out var cid) ? cid : Guid.NewGuid();
                publishContext.Headers.Set("CorrelationId", correlationId);
            });
        }
    }
}
