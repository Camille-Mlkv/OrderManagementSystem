using CartService.Application.DTOs;
using MediatR;

namespace CartService.Application.UseCases.Commands.AddItemToCart
{
    public record AddItemToCartCommand(Guid UserId, CartItemRequestDto Item): IRequest;
}
