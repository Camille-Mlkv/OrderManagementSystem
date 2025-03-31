using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrderService.Application.UseCases.Payments.Commands.CreateCheckoutSession;
using OrderService.Application.UseCases.Payments.Commands.HandleWebhook;

namespace OrderService.API.Controllers
{
    [Route("api/payments")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<PaymentController> _logger;

        public PaymentController(IMediator mediator, ILogger<PaymentController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        //[Authorize(Policy = "Client")]
        [HttpPost]
        public async Task<IActionResult> CreateCheckoutSession(Guid orderId, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new CreateCheckoutSessionCommand(orderId), cancellationToken);

            _logger.LogInformation($"Checkout session created for order {orderId}.");

            return Ok(result);
        }

        [HttpPost("webhook")]
        public async Task<IActionResult> StripeWebhook()
        {
            var json = await new StreamReader(Request.Body).ReadToEndAsync();
            var signature = Request.Headers["Stripe-Signature"];

            await Task.Run(async () =>
            {
                await _mediator.Send(new HandleWebhookCommand(json, signature!));
            });

            return Ok();
        }
    }
}
