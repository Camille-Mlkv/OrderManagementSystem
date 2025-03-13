using CartService.Application.Exceptions;
using CartService.Application.Specifications;
using CartService.Application.Specifications.Jobs;
using MediatR;

namespace CartService.Application.UseCases.Commands.DeleteItemFromCart
{
    public class DeleteItemFromCartHandler : IRequestHandler<DeleteItemFromCartCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICartJobService _jobService;

        public DeleteItemFromCartHandler(IUnitOfWork unitOfWork, ICartJobService jobService)
        {
            _unitOfWork = unitOfWork;
            _jobService = jobService;
        }

        public async Task Handle(DeleteItemFromCartCommand request, CancellationToken cancellationToken)
        {
            var cart = await _unitOfWork.CartRepository.GetCartAsync(request.UserId, cancellationToken);
            if (cart is null)
            {
                throw new BadRequestException("Bad request.", "You can't delete items from an empty cart."); 
            }

            cart.RemoveItem(request.MealId);

            await _jobService.DeleteJobAsync(request.UserId, cancellationToken);

            if (cart.Items.Count == 0)
            {
                await _unitOfWork.CartRepository.DeleteCartAsync(request.UserId, cancellationToken);
            }
            else
            {
                await _unitOfWork.CartRepository.SaveCartAsync(cart, cancellationToken);

                await _jobService.ScheduleJobAsync(request.UserId, cancellationToken);
            }

        }
    }
}
