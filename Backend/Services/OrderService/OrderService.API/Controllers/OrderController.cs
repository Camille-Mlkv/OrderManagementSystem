using MediatR;
using Microsoft.AspNetCore.Mvc;
using OrderService.Application.UseCases.Commands.CreateOrder;
using OrderService.Application.UseCases.Commands.DeletePendingOrder;
using OrderService.Application.UseCases.Queries.GetOpenedOrders;
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
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderCommand command, CancellationToken cancellationToken)
        {
            var orderId = await _mediator.Send(command,cancellationToken);

            return Ok(orderId);
        }

        [HttpDelete("{orderId}")]
        public async Task<IActionResult> DeletePendingOrder(Guid orderId, CancellationToken cancellationToken)
        {
            await _mediator.Send(new DeletePendingOrderCommand(orderId), cancellationToken);

            return NoContent();
        }

        [HttpGet("opened")]
        public async Task<IActionResult> GetOpenedOrders(CancellationToken cancellationToken)
        {
            var orders = await _mediator.Send(new GetOpenedOrdersQuery(), cancellationToken);

            return Ok(orders);
        }

        [HttpGet("{orderId}")]
        public async Task<IActionResult> GetOrderById(Guid orderId, CancellationToken cancellationToken)
        {
            var order = await _mediator.Send(new GetOrderByIdQuery(orderId), cancellationToken);

            return Ok(order);
        }
    }
}
