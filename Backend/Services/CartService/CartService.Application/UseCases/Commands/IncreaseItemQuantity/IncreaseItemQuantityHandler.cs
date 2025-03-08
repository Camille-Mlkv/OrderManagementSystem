using CartService.Application.Exceptions;
using CartService.Application.Specifications.Repositories;
using MediatR;

namespace CartService.Application.UseCases.Commands.IncreaseItemQuantity
{
    public class IncreaseItemQuantityHandler : IRequestHandler<IncreaseItemQuantityCommand>
    {
        private ICartRepository _cartRepository;

        public IncreaseItemQuantityHandler(ICartRepository cartRepository)
        {
            _cartRepository = cartRepository;
        }

        public async Task Handle(IncreaseItemQuantityCommand request, CancellationToken cancellationToken)
        {
            var existingItem = await _cartRepository.GetItemFromCartAsync(request.UserId, request.MealId);

            if (existingItem is null)
            {
                throw new NotFoundException("Item not found", $"Meal {request.MealId} not found in user {request.UserId} cart.");
            }

            await _cartRepository.RemoveItemFromCartAsync(request.UserId, existingItem);

            existingItem.Quantity += 1;

            await _cartRepository.AddItemToCartAsync(request.UserId, existingItem);

        }
    }
}
