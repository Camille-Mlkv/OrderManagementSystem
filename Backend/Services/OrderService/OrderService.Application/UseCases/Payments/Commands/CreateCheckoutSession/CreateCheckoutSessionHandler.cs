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
        private readonly UserService.GrpcServer.UserService.UserServiceClient _userClient;

        public CreateCheckoutSessionHandler(
            IPaymentService paymentService, 
            IOrderRepository orderRepository,
            UserService.GrpcServer.UserService.UserServiceClient userClient)
        {
            _paymentService = paymentService;
            _orderRepository = orderRepository;
            _userClient = userClient;
        }

        public async Task<PaymentResult> Handle(CreateCheckoutSessionCommand request, CancellationToken cancellationToken)
        {
            var order = await _orderRepository.GetByIdAsync(request.OrderId, cancellationToken);

            if (order == null)
            {
                throw new NotFoundException("Order with id doesn't exist");
            }

            var clientId = order.ClientId.ToString();
            var userRequest = new UserService.GrpcServer.UserRequest { UserId = clientId };
            var response = await _userClient.GetUserEmailByIdAsync(userRequest);
            var email = response.Email;

            var paymentResult = await _paymentService.CreateCheckoutSessionAsync(request.OrderId, order.TotalPrice, email);

            return paymentResult;
        }
    }
}
