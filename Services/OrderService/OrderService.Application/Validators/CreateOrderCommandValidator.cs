using FluentValidation;
using OrderService.Application.Orders.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderService.Application.Validators
{
    public class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
    {
        public CreateOrderCommandValidator()
        {
            RuleFor(x => x.CustomerId)
                .GreaterThan(0).WithMessage("CustomerId geçerli değil.");

            RuleFor(x => x.Items)
                .NotEmpty().WithMessage("Siparişe en az bir ürün eklenmelidir.");
        }
    }
}
