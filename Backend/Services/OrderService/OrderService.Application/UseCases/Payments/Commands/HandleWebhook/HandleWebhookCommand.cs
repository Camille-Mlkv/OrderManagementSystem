using MediatR;

namespace OrderService.Application.UseCases.Payments.Commands.HandleWebhook
{
    public record HandleWebhookCommand(string Json, string Signature):IRequest;
}
