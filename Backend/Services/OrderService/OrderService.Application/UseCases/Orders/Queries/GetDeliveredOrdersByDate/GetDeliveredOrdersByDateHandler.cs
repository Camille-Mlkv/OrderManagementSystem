using AutoMapper;
using MediatR;
using OrderService.Application.DTOs.Order;
using OrderService.Application.Specifications.Repositories;
using OrderService.Domain.Enums;

namespace OrderService.Application.UseCases.Orders.Queries.GetDeliveredOrdersByDate
{
    public class GetDeliveredOrdersByDateHandler : IRequestHandler<GetDeliveredOrdersByDateQuery, List<OrderDto>>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;

        public GetDeliveredOrdersByDateHandler(IOrderRepository orderRepository, IMapper mapper)
        {
            _orderRepository = orderRepository;
            _mapper = mapper;
        }

        public async Task<List<OrderDto>> Handle(GetDeliveredOrdersByDateQuery request, CancellationToken cancellationToken)
        {
            var newToDate = request.To.Date.AddDays(1).AddTicks(-1);

            var orders = await _orderRepository.GetListAsync(order => order.Status.Name == StatusName.Delivered
               && order.DeliveryDate >= request.From && order.DeliveryDate <= newToDate, cancellationToken);

            var ordersDtos = _mapper.Map<List<OrderDto>>(orders);

            return ordersDtos;
        }
    }
}
