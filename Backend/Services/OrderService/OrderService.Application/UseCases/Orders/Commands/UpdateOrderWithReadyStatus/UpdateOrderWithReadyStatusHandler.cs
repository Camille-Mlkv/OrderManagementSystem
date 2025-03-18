using MediatR;
using OrderService.Application.Exceptions;
using OrderService.Application.Specifications.Repositories;
using OrderService.Domain.Enums;

namespace OrderService.Application.UseCases.Orders.Commands.UpdateOrderWithReadyStatus
{
    public class UpdateOrderWithReadyStatusHandler : IRequestHandler<UpdateOrderWithReadyStatusCommand>
    {
        private readonly IOrderRepository _orderRepository;

        public UpdateOrderWithReadyStatusHandler(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
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
        }
    }
}
