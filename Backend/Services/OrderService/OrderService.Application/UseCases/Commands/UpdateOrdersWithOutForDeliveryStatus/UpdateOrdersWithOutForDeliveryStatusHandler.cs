using MediatR;
using OrderService.Application.Specifications.Repositories;
using OrderService.Domain.Enums;

namespace OrderService.Application.UseCases.Commands.UpdateOrdersWithOutForDeliveryStatus
{
    public class UpdateOrdersWithOutForDeliveryStatusHandler : IRequestHandler<UpdateOrdersWithOutForDeliveryStatusCommand>
    {
        private readonly IOrderRepository _orderRepository;

        public UpdateOrdersWithOutForDeliveryStatusHandler(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task Handle(UpdateOrdersWithOutForDeliveryStatusCommand request, CancellationToken cancellationToken)
        {
            var orders = await _orderRepository.GetListAsync(order => order.CourierId == request.CourierId 
               && order.Status.Name == StatusName.ReadyForDelivery, cancellationToken);

            foreach (var order in orders)
            {
                order.Status.Name = StatusName.OutForDelivery;
                await _orderRepository.UpdateAsync(order, cancellationToken);
            }
        }
    }
}
