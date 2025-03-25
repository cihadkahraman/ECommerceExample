using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationService.Application.Common.Models
{
    public record CorrelationId(Guid Value)
    {
        public override string ToString() => Value.ToString();
    }
}
