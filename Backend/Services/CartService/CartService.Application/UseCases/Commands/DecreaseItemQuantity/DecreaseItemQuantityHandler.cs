using CartService.Application.Exceptions;
using CartService.Application.Specifications.Repositories;
using MediatR;

namespace CartService.Application.UseCases.Commands.DecreaseItemQuantity
{
    public class DecreaseItemQuantityHandler : IRequestHandler<DecreaseItemQuantityCommand>
    {
        private ICartRepository _cartRepository;

        public DecreaseItemQuantityHandler(ICartRepository cartRepository)
        {
            _cartRepository = cartRepository;
        }

        public async Task Handle(DecreaseItemQuantityCommand request, CancellationToken cancellationToken)
        {
            var existingItem = await _cartRepository.GetItemFromCartAsync(request.UserId, request.MealId);

            if (existingItem is null)
            {
                throw new NotFoundException("Item not found", $"Meal {request.MealId} not found in user {request.UserId} cart.");
            }

            if (existingItem.Quantity != 1)
            {
                await _cartRepository.RemoveItemFromCartAsync(request.UserId, existingItem);

                existingItem.Quantity -= 1;

                await _cartRepository.AddItemToCartAsync(request.UserId, existingItem);
            }
            
        }
    }
}
