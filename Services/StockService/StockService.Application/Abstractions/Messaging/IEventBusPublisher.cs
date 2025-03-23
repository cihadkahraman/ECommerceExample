using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockService.Application.Abstractions.Messaging
{
    public interface IEventBusPublisher
    {
        Task PublishAsync<T>(T message, string queueName) where T : class;
    }
}
