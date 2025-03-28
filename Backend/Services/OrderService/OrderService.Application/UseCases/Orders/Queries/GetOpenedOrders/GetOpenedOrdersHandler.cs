﻿using AutoMapper;
using MediatR;
using OrderService.Application.DTOs.Order;
using OrderService.Application.Specifications.Repositories;
using OrderService.Domain.Enums;

namespace OrderService.Application.UseCases.Orders.Queries.GetOpenedOrders
{
    public class GetOpenedOrdersHandler : IRequestHandler<GetOpenedOrdersQuery, List<OpenedOrderDto>>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;

        public GetOpenedOrdersHandler(IOrderRepository orderRepository, IMapper mapper)
        {
            _orderRepository = orderRepository;
            _mapper = mapper;
        }

        public async Task<List<OpenedOrderDto>> Handle(GetOpenedOrdersQuery request, CancellationToken cancellationToken)
        {
            var orders = await _orderRepository.GetListAsync(order => order.CourierId == null 
               && order.Status.Name == StatusName.ReadyForDelivery, cancellationToken);

            var ordersDtos = _mapper.Map<List<OpenedOrderDto>>(orders);

            return ordersDtos;
        }
    }
}
