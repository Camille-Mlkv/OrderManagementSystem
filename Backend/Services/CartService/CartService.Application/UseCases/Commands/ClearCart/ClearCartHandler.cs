using CartService.Application.Specifications.Repositories;
using MediatR;

namespace CartService.Application.UseCases.Commands.ClearCart
{
    public class ClearCartHandler : IRequestHandler<ClearCartCommand>
    {
        private readonly ICartRepository _cartRepository;

        public ClearCartHandler(ICartRepository cartRepository)
        {
            _cartRepository = cartRepository;
        }

        public async Task Handle(ClearCartCommand request, CancellationToken cancellationToken)
        {
            await _cartRepository.ClearCartAsync(request.UserId);
        }
    }
}
