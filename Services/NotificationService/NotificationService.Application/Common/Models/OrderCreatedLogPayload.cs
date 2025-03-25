using NotificationService.Application.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace NotificationService.Application.Common.Models
{
    public record OrderCreatedLogPayload
    (
    Guid OrderId,
    int CustomerId,
    DateTime CreatedAt,
    List<OrderItemDto> Items);
}
