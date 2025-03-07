using CartService.Application.DTOs;
using MediatR;

namespace CartService.Application.UseCases.Queries.GetItemByIdFromCart
{
    public record GetItemByIdFromCartQuery(string UserId, Guid MealId) : IRequest<CartItemDto>;
}
