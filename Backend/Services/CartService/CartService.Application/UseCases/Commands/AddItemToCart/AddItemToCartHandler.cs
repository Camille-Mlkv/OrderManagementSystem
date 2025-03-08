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
            var existingItem = await _cartRepository.GetItemFromCartAsync(request.UserId, request.Item.MealId);

            if (existingItem != null)
            {
                await _cartRepository.RemoveItemFromCartAsync(request.UserId, existingItem);

                existingItem.Quantity += request.Item.Quantity; 

                await _cartRepository.AddItemToCartAsync(request.UserId, existingItem);
            }
            else
            {
                var newItem = _mapper.Map<CartItem>(request.Item);

                await _cartRepository.AddItemToCartAsync(request.UserId, newItem);
            }
        }
    }
}
