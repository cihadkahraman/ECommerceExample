using MassTransit;
using StockService.Application.Abstractions.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockService.Infrastructure.Messaging
{
    public class MassTransitEventBusPublisher : IEventBusPublisher
    {
        private readonly IPublishEndpoint _publishEndpoint;

        public MassTransitEventBusPublisher(IPublishEndpoint publishEndpoint)
        {
            _publishEndpoint = publishEndpoint;
        }

        public async Task PublishAsync<T>(T message) where T : class
        {
            await _publishEndpoint.Publish(message);
        }
    }
}
