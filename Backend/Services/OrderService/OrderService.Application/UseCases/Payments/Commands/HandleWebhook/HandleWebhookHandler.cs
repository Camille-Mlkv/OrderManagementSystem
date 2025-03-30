using MediatR;
using OrderService.Application.DTOs.Order;
using OrderService.Application.Specifications.Repositories;
using OrderService.Application.Specifications.Services;
using OrderService.Domain.Enums;

namespace OrderService.Application.UseCases.Payments.Commands.HandleWebhook
{
    public class HandleWebhookHandler : IRequestHandler<HandleWebhookCommand>
    {
        private readonly IPaymentService _paymentService;
        private readonly IOrderRepository _orderRepository;
        private readonly IMessageService _messageService;

        public HandleWebhookHandler(
            IPaymentService paymentService, 
            IOrderRepository orderRepository,
            IMessageService messageService)
        {
            _paymentService = paymentService;
            _orderRepository = orderRepository;
            _messageService = messageService;
        }
        public async Task Handle(HandleWebhookCommand request, CancellationToken cancellationToken)
        {
            var id = _paymentService.HandleWebhook(request.Json, request.Signature);

            if (id == null)
            {
                throw new Exception();
            }

            var order = await _orderRepository.GetByIdAsync(id.Value, cancellationToken);

            order.Status.Name = StatusName.InProgress;

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
