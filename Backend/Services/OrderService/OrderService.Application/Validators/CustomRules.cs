using FluentValidation;
using OrderService.Domain.Enums;

namespace OrderService.Application.Validators
{
    public static class CustomRules
    {
        public static IRuleBuilderOptions<T, string> MustBeAValidOrderStatus<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder.Must(status => Enum.TryParse(typeof(StatusName), status, true, out _))
                .WithMessage("Invalid status: specify either InProgress, ReadyForDelivery, OutForDelivery or Delivered.");
        }
    }
}
