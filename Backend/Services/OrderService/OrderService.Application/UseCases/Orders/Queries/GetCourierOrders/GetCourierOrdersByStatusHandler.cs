using AutoMapper;
using MediatR;
using OrderService.Application.DTOs.Order;
using OrderService.Application.Specifications.Repositories;

namespace OrderService.Application.UseCases.Orders.Queries.GetCourierOrders
{
    public class GetCourierOrdersByStatusHandler : IRequestHandler<GetCourierOrdersByStatusQuery, List<OrderDto>>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;

        public GetCourierOrdersByStatusHandler(IOrderRepository orderRepository, IMapper mapper)
        {
            _orderRepository = orderRepository;
            _mapper = mapper;
        }

        public async Task<List<OrderDto>> Handle(GetCourierOrdersByStatusQuery request, CancellationToken cancellationToken)
        {
            var orders = await _orderRepository.GetListAsync(order => order.CourierId == request.CourierId 
               && order.Status.Name.ToString() == request.Status, cancellationToken);

            var ordersDtos = _mapper.Map<List<OrderDto>>(orders);

            return ordersDtos;
        }
    }
}
