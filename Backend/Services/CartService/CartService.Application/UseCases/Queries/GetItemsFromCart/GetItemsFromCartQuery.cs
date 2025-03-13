using CartService.Application.DTOs;
using MediatR;

namespace CartService.Application.UseCases.Queries.GetItemsFromCart
{
    public record GetItemsFromCartQuery(string UserId) : IRequest<List<CartItemDto>>;
}
