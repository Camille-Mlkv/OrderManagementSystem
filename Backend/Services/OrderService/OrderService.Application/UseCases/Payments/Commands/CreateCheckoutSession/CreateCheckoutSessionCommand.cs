using MediatR;
using OrderService.Application.DTOs.Payment;

namespace OrderService.Application.UseCases.Payments.Commands.CreateCheckoutSession
{
    public record CreateCheckoutSessionCommand(Guid OrderId): IRequest<PaymentResult>;
}
