using MediatR;
using OrderService.Application.Specifications.Repositories;
using OrderService.Application.Specifications.Services;

namespace OrderService.Application.UseCases.Payments.Commands.HandleWebhook
{
    public class HandleWebhookHandler : IRequestHandler<HandleWebhookCommand>
    {
        private readonly IPaymentService _paymentService;
        private readonly IOrderRepository _orderRepository;

        public HandleWebhookHandler(IPaymentService paymentService, IOrderRepository orderRepository)
        {
            _paymentService = paymentService;
            _orderRepository = orderRepository;
        }
        public async Task Handle(HandleWebhookCommand request, CancellationToken cancellationToken)
        {
            var id = _paymentService.HandleWebhook(request.Json, request.Signature);

            if (id == null)
            {
                throw new Exception();
            }

            var order = await _orderRepository.GetByIdAsync(id.Value, cancellationToken);

            order.Status.Name = Domain.Enums.StatusName.InProgress;

            await _orderRepository.UpdateAsync(order, cancellationToken);
        }
    }
}
