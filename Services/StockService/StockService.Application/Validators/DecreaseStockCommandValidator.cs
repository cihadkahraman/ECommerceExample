using FluentValidation;
using StockService.Application.Orders.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockService.Application.Validators
{
    public class DecreaseStockCommandValidator : AbstractValidator<DecreaseStockCommand>
    {
        public DecreaseStockCommandValidator()
        {
            RuleFor(x => x.ProductId)
                .GreaterThan(0)
                .WithMessage("ProductId pozitif bir değer olmalıdır!");

            RuleFor(x => x.Quantity)
                .GreaterThan(0)
                .WithMessage("Quantity pozitif bir değer olmalıdır!");
        }
    }
}
