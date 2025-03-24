using MassTransit;
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

        public MassTransitEventBusPublisher(IBus bus)
        {
            _bus = bus;
        }

        public async Task PublishAsync<T>(T message) where T : class
        {
            await _bus.Publish(message);
        }
    }
}
