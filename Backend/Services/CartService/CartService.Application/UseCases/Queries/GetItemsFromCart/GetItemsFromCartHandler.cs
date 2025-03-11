using AutoMapper;
using CartService.Application.DTOs;
using CartService.Application.Specifications;
using MediatR;

namespace CartService.Application.UseCases.Queries.GetItemsFromCart
{
    public class GetItemsFromCartHandler: IRequestHandler<GetItemsFromCartQuery, List<CartItemDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetItemsFromCartHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<List<CartItemDto>> Handle(GetItemsFromCartQuery request, CancellationToken cancellationToken)
        {
            var cart = await _unitOfWork.CartRepository.GetCartAsync(request.UserId, cancellationToken);

            if (cart is null)
            {
                return new List<CartItemDto>();
            }

            var itemsDtos = _mapper.Map<List<CartItemDto>>(cart.Items);

            return itemsDtos;
        }
    }
}
