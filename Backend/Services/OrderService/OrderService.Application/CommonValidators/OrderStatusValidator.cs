using FluentValidation;
using OrderService.Domain.Enums;

namespace OrderService.Application.CommonValidators
{
    public class OrderStatusValidator : AbstractValidator<string>
    {
        public OrderStatusValidator()
        {
            RuleFor(status => status)
                .Must(status => Enum.TryParse(typeof(StatusName), status, true, out _))
                .WithMessage("Invalid status: specify either InProgress, ReadyForDelivery, OutForDelivery or Delivered.");
        }
    }
}
