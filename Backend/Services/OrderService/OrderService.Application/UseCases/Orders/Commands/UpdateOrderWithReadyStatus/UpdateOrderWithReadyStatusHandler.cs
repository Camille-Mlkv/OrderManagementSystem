using MediatR;
using OrderService.Application.DTOs.Order;
using OrderService.Application.Exceptions;
using OrderService.Application.Specifications.Repositories;
using OrderService.Application.Specifications.Services;
using OrderService.Domain.Enums;

namespace OrderService.Application.UseCases.Orders.Commands.UpdateOrderWithReadyStatus
{
    public class UpdateOrderWithReadyStatusHandler : IRequestHandler<UpdateOrderWithReadyStatusCommand>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMessageService _messageService;

        public UpdateOrderWithReadyStatusHandler(IOrderRepository orderRepository, IMessageService messageService)
        {
            _orderRepository = orderRepository;
            _messageService = messageService;
        }

        public async Task Handle(UpdateOrderWithReadyStatusCommand request, CancellationToken cancellationToken)
        {
            var order = await _orderRepository.GetByIdAsync(request.OrderId, cancellationToken);

            if (order is null)
            {
                throw new NotFoundException($"Order with Id {request.OrderId} not found.");
            }

            if (order.Status.Name != StatusName.InProgress)
            {
                throw new BadRequestException($"Bad request for order {request.OrderId}.", "Status can be set to ready only if the order is prepared.");
            }

            order.Status.Name = StatusName.ReadyForDelivery;

            await _orderRepository.UpdateAsync(order, cancellationToken);

            var orderStatus = new OrderStatusDto
            {
                UserId = order.ClientId,
                OrderNumber = order.OrderNumber,
                OrderStatus = order.Status.Name.ToString(),
            };

            await _messageService.PublishAsync(orderStatus);
        }
    }
}
