using AutoMapper;
using CartService.Application.DTOs;
using CartService.Application.Specifications.Repositories;
using MediatR;

namespace CartService.Application.UseCases.Queries.GetItemByIdFromCart
{
    public class GetItemByIdFromCartHandler : IRequestHandler<GetItemByIdFromCartQuery, CartItemDto>
    {
        private readonly ICartRepository _cartRepository;
        private readonly IMapper _mapper;

        public GetItemByIdFromCartHandler(ICartRepository cartRepository, IMapper mapper)
        {
            _cartRepository = cartRepository;
            _mapper = mapper;
        }
        public async Task<CartItemDto> Handle(GetItemByIdFromCartQuery request, CancellationToken cancellationToken)
        {
            var existingItem = await _cartRepository.GetItemFromCartAsync(request.UserId, request.MealId);
            
            return _mapper.Map<CartItemDto>(existingItem);
        }
    }
}
