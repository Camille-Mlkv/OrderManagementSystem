using FluentValidation;
using OrderService.Application.CommonValidators;

namespace OrderService.Application.UseCases.Queries.GetClientOrdersByStatus
{
    public class GetClientOrdersByStatusQueryValidator : AbstractValidator<GetClientOrdersByStatusQuery>
    {
        public GetClientOrdersByStatusQueryValidator()
        {
            RuleFor(x => x.Status).SetValidator(new OrderStatusValidator());
        }
    }
}
