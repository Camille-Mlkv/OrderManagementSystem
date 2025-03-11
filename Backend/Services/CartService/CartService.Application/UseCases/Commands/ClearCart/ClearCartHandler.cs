using CartService.Application.Specifications;
using CartService.Application.Specifications.Jobs;
using MediatR;

namespace CartService.Application.UseCases.Commands.ClearCart
{
    public class ClearCartHandler : IRequestHandler<ClearCartCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICartJobService _jobService;

        public ClearCartHandler(IUnitOfWork unitOfWork, ICartJobService jobService)
        {
            _unitOfWork = unitOfWork;
            _jobService = jobService;
        }

        public async Task Handle(ClearCartCommand request, CancellationToken cancellationToken)
        {
            await _unitOfWork.CartRepository.DeleteCartAsync(request.UserId, cancellationToken);

            await _jobService.DeleteJobAsync(request.UserId, cancellationToken);
        }
    }
}
