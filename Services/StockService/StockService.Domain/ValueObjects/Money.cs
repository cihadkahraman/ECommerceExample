using StockService.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockService.Domain.ValueObjects
{
    public record Money(decimal Amount, string Currency = "TRY")
    {
        public static Money operator +(Money a, Money b)
        {
            if (a.Currency != b.Currency)
                throw new DomainException("Para birimleri eşleşmiyor.");

            return new Money(a.Amount + b.Amount, a.Currency);
        }
    }
}
