using CartService.Application.Exceptions;
using CartService.Application.Specifications.Repositories;
using MediatR;

namespace CartService.Application.UseCases.Commands.DeleteItemFromCart
{
    public class DeleteItemFromCartHandler : IRequestHandler<DeleteItemFromCartCommand>
    {
        private readonly ICartRepository _cartRepository;

        public DeleteItemFromCartHandler(ICartRepository cartRepository)
        {
            _cartRepository = cartRepository;
        }

        public async Task Handle(DeleteItemFromCartCommand request, CancellationToken cancellationToken)
        {
            var existingItem = await _cartRepository.GetItemFromCartAsync(request.UserId, request.MealId);

            if (existingItem is null)
            {
                throw new NotFoundException("Item not found", $"Meal {request.MealId} not found in user {request.UserId} cart.");
            }

            await _cartRepository.RemoveItemFromCartAsync(request.UserId, existingItem);
        }
    }
}
