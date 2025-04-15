using CartService.Application.DTOs;
using MediatR;

namespace CartService.Application.UseCases.Commands.UpdateItemQuantity
{
    public record UpdateItemQuantityCommand(Guid UserId, CartItemDto Item): IRequest;
}
