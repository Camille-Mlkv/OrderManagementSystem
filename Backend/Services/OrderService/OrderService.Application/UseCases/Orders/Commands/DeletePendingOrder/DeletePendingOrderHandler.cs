using MediatR;
using OrderService.Application.Exceptions;
using OrderService.Application.Specifications.Repositories;
using OrderService.Domain.Enums;

namespace OrderService.Application.UseCases.Orders.Commands.DeletePendingOrder
{
    public class DeletePendingOrderHandler : IRequestHandler<DeletePendingOrderCommand>
    {
        private readonly IOrderRepository _orderRepository;

        public DeletePendingOrderHandler(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task Handle(DeletePendingOrderCommand request, CancellationToken cancellationToken)
        {
            var order = await _orderRepository.GetByIdAsync(request.OrderId, cancellationToken);

            if (order is null)
            {
                throw new BadRequestException("Bad request.", $"Order with ID {request.OrderId} doesn't exist.");
            }

            if (order.Status.Name != StatusName.Pending)
            {
                throw new BadRequestException("Bad request.", $"Order with ID {request.OrderId} is not pending.");
            }

            await _orderRepository.DeleteAsync(request.OrderId, cancellationToken);
        }
    }
}
