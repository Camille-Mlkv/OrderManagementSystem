using AutoMapper;
using MediatR;
using OrderService.Application.DTOs.Order;
using OrderService.Application.Specifications.Repositories;

namespace OrderService.Application.UseCases.Orders.Queries.GetOrdersByStatus
{
    public class GetOrdersByStatusHandler : IRequestHandler<GetOrdersByStatusQuery, List<OrderDto>>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;

        public GetOrdersByStatusHandler(IOrderRepository orderRepository, IMapper mapper)
        {
            _orderRepository = orderRepository;
            _mapper = mapper;
        }

        public async Task<List<OrderDto>> Handle(GetOrdersByStatusQuery request, CancellationToken cancellationToken)
        {
            var orders = await _orderRepository.GetListAsync(order => order.Status.Name.ToString() == request.Status, cancellationToken);

            var ordersDtos = _mapper.Map<List<OrderDto>>(orders);

            return ordersDtos;
        }
    }
}
