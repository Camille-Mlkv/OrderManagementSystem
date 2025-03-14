using FluentValidation;
using OrderService.Application.CommonValidators;

namespace OrderService.Application.UseCases.Queries.GetOrdersByStatus
{
    public class GetOrdersByStatusQueryValidator : AbstractValidator<GetOrdersByStatusQuery>
    {
        public GetOrdersByStatusQueryValidator()
        {
            RuleFor(x => x.Status).SetValidator(new OrderStatusValidator());
        }
    }
}
