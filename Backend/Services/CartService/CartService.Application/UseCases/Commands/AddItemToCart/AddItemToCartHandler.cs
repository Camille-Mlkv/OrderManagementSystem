using AutoMapper;
using CartService.Application.Specifications.Repositories;
using CartService.Domain.Entities;
using MediatR;

namespace CartService.Application.UseCases.Commands.AddItemToCart
{
    public class AddItemToCartHandler : IRequestHandler<AddItemToCartCommand>
    {
        private readonly ICartRepository _cartRepository;
        private readonly IMapper _mapper;

        public AddItemToCartHandler(ICartRepository cartRepository, IMapper mapper)
        {
            _cartRepository = cartRepository;
            _mapper = mapper;
        }

        public async Task Handle(AddItemToCartCommand request, CancellationToken cancellationToken)
        {
            var newItem = _mapper.Map<CartItem>(request.Item);

            await _cartRepository.AddItemToCartAsync(request.UserId, newItem);
        }
    }
}
