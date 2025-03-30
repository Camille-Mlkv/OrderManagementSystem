using MediatR;
using OrderService.Application.DTOs.Order;
using OrderService.Application.Specifications.Repositories;
using OrderService.Application.Specifications.Services;
using OrderService.Domain.Enums;
using OrderService.Application.Exceptions;
using OrderService.Domain.Entities;

namespace OrderService.Application.UseCases.Orders.Commands.UpdateOrdersWithOutForDeliveryStatus
{
    public class UpdateOrdersWithOutForDeliveryStatusHandler : IRequestHandler<UpdateOrdersWithOutForDeliveryStatusCommand>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMessageService _messageService;

        public UpdateOrdersWithOutForDeliveryStatusHandler(IOrderRepository orderRepository, IMessageService messageService)
        {
            _orderRepository = orderRepository;
            _messageService = messageService;
        }

        public async Task Handle(UpdateOrdersWithOutForDeliveryStatusCommand request, CancellationToken cancellationToken)
        {
            var orders = await _orderRepository.GetListAsync( order =>
                order.CourierId == request.CourierId
                && order.Status.Name == StatusName.ReadyForDelivery, 
                cancellationToken);

            if (orders.Count() == 0)
            {
                throw new BadRequestException("Bad request", "This courier doesn't have ready orders.");
            }

            foreach (var order in orders)
            {
                order.Status.Name = StatusName.OutForDelivery;
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
}
