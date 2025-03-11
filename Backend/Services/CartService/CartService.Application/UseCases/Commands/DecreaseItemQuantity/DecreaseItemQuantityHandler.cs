using CartService.Application.Exceptions;
using CartService.Application.Specifications;
using CartService.Application.Specifications.Jobs;
using MediatR;

namespace CartService.Application.UseCases.Commands.DecreaseItemQuantity
{
    public class DecreaseItemQuantityHandler : IRequestHandler<DecreaseItemQuantityCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICartJobService _jobService;

        public DecreaseItemQuantityHandler(IUnitOfWork unitOfWork, ICartJobService jobService)
        {
            _unitOfWork = unitOfWork;
            _jobService = jobService;
        }

        public async Task Handle(DecreaseItemQuantityCommand request, CancellationToken cancellationToken)
        {
            var cart = await _unitOfWork.CartRepository.GetCartAsync(request.UserId, cancellationToken);

            if (cart is null)
            {
                throw new BadRequestException("Bad request.", "Cart is empty.");
            }

            var existingItem = cart.GetItemById(request.MealId);

            if (existingItem is null)
            {
                throw new NotFoundException("Item not found", $"Meal {request.MealId} not found in user {request.UserId} cart.");
            }

            if (existingItem.Quantity > 1)
            {
                existingItem.Quantity -= 1;
                await _unitOfWork.CartRepository.SaveCartAsync(cart, cancellationToken);
            }

            await _jobService.DeleteJobAsync(request.UserId, cancellationToken); 
            await _jobService.ScheduleJobAsync(request.UserId, cancellationToken);
        }
    }
}
