using AutoMapper;
using CartService.Application.DTOs;
using CartService.Application.Specifications.Repositories;
using MediatR;

namespace CartService.Application.UseCases.Queries.GetItemsFromCart
{
    public class GetItemsFromCartHandler: IRequestHandler<GetItemsFromCartQuery, List<CartItemDto>>
    {
        private readonly ICartRepository _cartRepository;
        private readonly IMapper _mapper;

        public GetItemsFromCartHandler(ICartRepository cartRepository, IMapper mapper)
        {
            _cartRepository = cartRepository;
            _mapper = mapper;
        }

        public async Task<List<CartItemDto>> Handle(GetItemsFromCartQuery query, CancellationToken cancellationToken)
        {
            var cartItems = await _cartRepository.GetCartItemsAsync(query.UserId);

            var cartItemsDtos = _mapper.Map<List<CartItemDto>>(cartItems);

            return cartItemsDtos;
        }
    }
}
