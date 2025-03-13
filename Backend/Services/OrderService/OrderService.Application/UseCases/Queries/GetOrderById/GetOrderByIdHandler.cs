using AutoMapper;
using MediatR;
using OrderService.Application.DTOs.Order;
using OrderService.Application.Specifications.Repositories;

namespace OrderService.Application.UseCases.Queries.GetOrderById
{
    public class GetOrderByIdHandler : IRequestHandler<GetOrderByIdQuery, OrderDto>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;

        public GetOrderByIdHandler(IOrderRepository orderRepository, IMapper mapper)
        {
            _orderRepository = orderRepository;
            _mapper = mapper;
        }

        public async Task<OrderDto> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
        {
            var order = await _orderRepository.GetByIdAsync(request.Id, cancellationToken);

            var orderDto = _mapper.Map<OrderDto>(order);    

            return orderDto;
        }
    }
}
