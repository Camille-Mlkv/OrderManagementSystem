using MediatR;
using Microsoft.AspNetCore.Mvc;
using OrderService.Application.UseCases.Commands.CreateOrder;
using OrderService.Application.UseCases.Commands.DeletePendingOrder;
using OrderService.Application.UseCases.Commands.UpdateOrderWithCourierId;
using OrderService.Application.UseCases.Commands.UpdateOrderWithReadyStatus;
using OrderService.Application.UseCases.Queries.GetOrderById;

namespace OrderService.API.Controllers
{
    [ApiController]
    [Route("api/orders")]
    public class OrderController : ControllerBase
    {
        private readonly IMediator _mediator;

        public OrderController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        // client
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderCommand command, CancellationToken cancellationToken)
        {
            var orderId = await _mediator.Send(command,cancellationToken);

            return Ok(orderId);
        }

        [HttpDelete("{orderId}")]
        // client
        public async Task<IActionResult> DeletePendingOrder(Guid orderId, CancellationToken cancellationToken)
        {
            await _mediator.Send(new DeletePendingOrderCommand(orderId), cancellationToken);

            return NoContent();
        }

        [HttpGet("{orderId}")]
        public async Task<IActionResult> GetOrderById(Guid orderId, CancellationToken cancellationToken)
        {
            var order = await _mediator.Send(new GetOrderByIdQuery(orderId), cancellationToken);

            return Ok(order);
        }

        [HttpPatch("{orderId:guid}/assign-courier")]
        // courier
        public async Task<IActionResult> UpdateOrderWithCourierId(Guid orderId, Guid courierId, CancellationToken cancellationToken)
        {
            // later courierId will be retrieved from the token

            await _mediator.Send(new UpdateOrderWithCourierIdCommand(courierId, orderId), cancellationToken);

            return NoContent();
        }

        [HttpPatch("{orderId:guid}/set-ready")]
        // admin
        public async Task<IActionResult> UpdateOrderWithReadyStatus(Guid orderId, CancellationToken cancellationToken)
        {
            await _mediator.Send(new UpdateOrderWithReadyStatusCommand(orderId), cancellationToken);

            return NoContent();
        }

    }
}
