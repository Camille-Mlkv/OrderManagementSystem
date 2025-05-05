using AutoMapper;
using MediatR;
using OrderService.Application.DTOs.Order;
using OrderService.Application.Exceptions;
using OrderService.Application.Specifications.Repositories;
using OrderService.Application.Specifications.Services;
using OrderService.Domain.Enums;

namespace OrderService.Application.UseCases.Orders.Commands.UpdateOrderWithCourierId
{
    public class UpdateOrderWithCourierIdHandler : IRequestHandler<UpdateOrderWithCourierIdCommand>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IOrderNotificationService _notifier;
        private readonly IMapper _mapper;

        public UpdateOrderWithCourierIdHandler(
           IOrderRepository orderRepository,
           IOrderNotificationService notifier,
           IMapper mapper)
        {
            _orderRepository = orderRepository;
            _notifier = notifier;
            _mapper = mapper;
        }

        public async Task Handle(UpdateOrderWithCourierIdCommand request, CancellationToken cancellationToken)
        {
            var order = await _orderRepository.GetByIdAsync(request.OrderId, cancellationToken);

            if (order is null)
            {
                throw new NotFoundException($"Order with Id {request.OrderId} not found.");
            }

            if (order.Status.Name != StatusName.ReadyForDelivery)
            {
                throw new BadRequestException($"Bad request for order {request.OrderId}.", "Courier can respond only to orders that are ready for delivery.");
            }

            order.CourierId = request.CourierId;

            await _orderRepository.UpdateAsync(order,cancellationToken);

            var orderDto = _mapper.Map<OrderDto>(order);

            await _notifier.NotifyOrderUpdatedAsync(orderDto);
        }
    }
}
