using CartService.Application.Specifications.Jobs;
using CartService.Application.Specifications;
using MediatR;
using CartService.Application.Exceptions;

namespace CartService.Application.UseCases.Commands.UpdateItemQuantity
{
    public class UpdateItemQuantityHandler : IRequestHandler<UpdateItemQuantityCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICartJobService _jobService;

        public UpdateItemQuantityHandler(IUnitOfWork unitOfWork, ICartJobService jobService)
        {
            _unitOfWork = unitOfWork;
            _jobService = jobService;
        }

        public async Task Handle(UpdateItemQuantityCommand request, CancellationToken cancellationToken)
        {
            var cart = await _unitOfWork.CartRepository.GetCartAsync(request.UserId, cancellationToken);

            if (cart is null)
            {
                throw new BadRequestException("Bad request.", "Cart is empty.");
            }

            var existingItem = cart.GetItemById(request.Item.MealId);

            if (existingItem is null)
            {
                throw new NotFoundException("Item not found", $"Meal {request.Item.MealId} not found in user {request.UserId} cart.");
            }

            existingItem.Quantity = request.Item.Quantity;
            await _unitOfWork.CartRepository.SaveCartAsync(cart, cancellationToken);

            await _jobService.DeleteJobAsync(request.UserId, cancellationToken);
            await _jobService.ScheduleJobAsync(request.UserId, cancellationToken);
        }
    }
}
