using MediatR;
using OrderService.Application.DTOs.Payment;
using OrderService.Application.Exceptions;
using OrderService.Application.Specifications.Repositories;
using OrderService.Application.Specifications.Services;

namespace OrderService.Application.UseCases.Payments.Commands.CreateCheckoutSession
{
    public class CreateCheckoutSessionHandler : IRequestHandler<CreateCheckoutSessionCommand, PaymentResult>
    {
        private readonly IPaymentService _paymentService;
        private readonly IOrderRepository _orderRepository;

        public CreateCheckoutSessionHandler(IPaymentService paymentService, IOrderRepository orderRepository)
        {
            _paymentService = paymentService;
            _orderRepository = orderRepository;
        }
        public async Task<PaymentResult> Handle(CreateCheckoutSessionCommand request, CancellationToken cancellationToken)
        {
            var order = await _orderRepository.GetByIdAsync(request.OrderId, cancellationToken);

            if (order == null)
            {
                throw new NotFoundException("Order with id doesn't exist");
            }

            // for retrieving email via gRPC
            var clientId = order.ClientId;

            // value is hardcoded for testing purposes (will be removed when adding gRPC)
            var email = "testclient@gmail.com";

            // price is hardcoded for testing purposes because meals are not retrieved yet, so price is currently 0
            var paymentResult = await _paymentService.CreateCheckoutSessionAsync(request.OrderId, 22, email);

            return paymentResult;
        }
    }
}
