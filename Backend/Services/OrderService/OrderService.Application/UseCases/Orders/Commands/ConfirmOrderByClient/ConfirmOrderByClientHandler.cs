using AutoMapper;
using MediatR;
using OrderService.Application.DTOs.Order;
using OrderService.Application.Exceptions;
using OrderService.Application.Specifications.Repositories;
using OrderService.Application.Specifications.Services;
using OrderService.Domain.Enums;

namespace OrderService.Application.UseCases.Orders.Commands.ConfirmOrderByClient
{
    public class ConfirmOrderByClientHandler : IRequestHandler<ConfirmOrderByClientCommand>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMessageService _messageService;
        private readonly IOrderNotificationService _notifier;
        private readonly IMapper _mapper;

        public ConfirmOrderByClientHandler(
           IOrderRepository orderRepository, 
           IMessageService messageService, 
           IOrderNotificationService notifier,
           IMapper mapper)
        {
            _orderRepository = orderRepository;
            _messageService = messageService;
            _notifier = notifier;
            _mapper = mapper;
        }

        public async Task Handle(ConfirmOrderByClientCommand request, CancellationToken cancellationToken)
        {
            var order = await _orderRepository.GetByIdAsync(request.OrderId, cancellationToken);

            if (order is null)
            {
                throw new NotFoundException($"Order with Id {request.OrderId} not found.");
            }

            if (order.Status.Name != StatusName.OutForDelivery)
            {
                throw new BadRequestException("Bad request.", "This order can't be confirmed as it's not delivered yet.");
            }

            order.ConfirmedByClient = true;

            if(order.ConfirmedByClient && order.ConfirmedByCourier)
            {
                order.Status.Name = StatusName.Delivered;
                order.DeliveryDate = DateTime.UtcNow;

                var orderStatus = new OrderStatusDto
                {
                    UserId = order.ClientId,
                    OrderNumber = order.OrderNumber,
                    OrderStatus = order.Status.Name.ToString(),
                };

                await _messageService.PublishAsync(orderStatus);
            }

            await _orderRepository.UpdateAsync(order, cancellationToken);

            var orderDto = _mapper.Map<OrderDto>(order);
            await _notifier.NotifyOrderUpdatedAsync(orderDto);
        }
    }
}
