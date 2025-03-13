using AutoMapper;
using MediatR;
using OrderService.Application.DTOs.Order;
using OrderService.Application.Specifications.Repositories;
using OrderService.Domain.Enums;

namespace OrderService.Application.UseCases.Queries.GetCourierOrders
{
    public class GetCourierOrdersHandler : IRequestHandler<GetCourierOrdersQuery, List<OrderDto>>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;

        public GetCourierOrdersHandler(IOrderRepository orderRepository, IMapper mapper)
        {
            _orderRepository = orderRepository;
            _mapper = mapper;
        }

        public async Task<List<OrderDto>> Handle(GetCourierOrdersQuery request, CancellationToken cancellationToken)
        {
            var orders = await _orderRepository.GetListAsync(order => order.CourierId == request.CourierId && order.Status.Name == StatusName.OutForDelivery, cancellationToken);

            var ordersDtos = _mapper.Map<List<OrderDto>>(orders);

            return ordersDtos;
        }
    }
}
