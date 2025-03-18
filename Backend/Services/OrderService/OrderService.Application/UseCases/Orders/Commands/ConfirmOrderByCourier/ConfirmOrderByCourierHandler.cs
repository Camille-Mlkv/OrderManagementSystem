using MediatR;
using OrderService.Application.Exceptions;
using OrderService.Application.Specifications.Repositories;
using OrderService.Domain.Enums;

namespace OrderService.Application.UseCases.Orders.Commands.ConfirmOrderByCourier
{
    public class ConfirmOrderByCourierHandler: IRequestHandler<ConfirmOrderByCourierCommand>
    {
        private readonly IOrderRepository _orderRepository;

        public ConfirmOrderByCourierHandler(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }
        public async Task Handle(ConfirmOrderByCourierCommand request, CancellationToken cancellationToken)
        {
            var order = await _orderRepository.GetByIdAsync(request.OrderId, cancellationToken);

            if (order is null)
            {
                throw new NotFoundException($"Order with Id {request.OrderId} not found.");
            }

            order.ConfirmedByCourier = true;

            if (order.ConfirmedByClient && order.ConfirmedByCourier)
            {
                order.Status.Name = StatusName.Delivered;
                order.DeliveryDate = DateTime.UtcNow;
            }

            await _orderRepository.UpdateAsync(order, cancellationToken);
        }
    }
}
