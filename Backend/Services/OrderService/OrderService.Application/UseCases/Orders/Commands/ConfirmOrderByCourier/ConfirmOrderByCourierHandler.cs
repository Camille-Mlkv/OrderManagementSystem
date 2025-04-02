using MediatR;
using OrderService.Application.DTOs.Order;
using OrderService.Application.Exceptions;
using OrderService.Application.Specifications.Repositories;
using OrderService.Application.Specifications.Services;
using OrderService.Domain.Enums;

namespace OrderService.Application.UseCases.Orders.Commands.ConfirmOrderByCourier
{
    public class ConfirmOrderByCourierHandler: IRequestHandler<ConfirmOrderByCourierCommand>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMessageService _messageService;

        public ConfirmOrderByCourierHandler(IOrderRepository orderRepository, IMessageService messageService)
        {
            _orderRepository = orderRepository;
            _messageService = messageService;
        }

        public async Task Handle(ConfirmOrderByCourierCommand request, CancellationToken cancellationToken)
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

            order.ConfirmedByCourier = true;

            if (order.ConfirmedByClient && order.ConfirmedByCourier)
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
        }
    }
}
