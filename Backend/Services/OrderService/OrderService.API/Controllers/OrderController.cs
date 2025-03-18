using MediatR;
using Microsoft.AspNetCore.Mvc;
using OrderService.Application.UseCases.Orders.Commands.ConfirmOrderByClient;
using OrderService.Application.UseCases.Orders.Commands.ConfirmOrderByCourier;
using OrderService.Application.UseCases.Orders.Commands.CreateOrder;
using OrderService.Application.UseCases.Orders.Commands.DeletePendingOrder;
using OrderService.Application.UseCases.Orders.Commands.UpdateOrdersWithOutForDeliveryStatus;
using OrderService.Application.UseCases.Orders.Commands.UpdateOrderWithCourierId;
using OrderService.Application.UseCases.Orders.Commands.UpdateOrderWithReadyStatus;
using OrderService.Application.UseCases.Orders.Queries.GetClientOrdersByStatus;
using OrderService.Application.UseCases.Orders.Queries.GetCourierOrders;
using OrderService.Application.UseCases.Orders.Queries.GetOpenedOrders;
using OrderService.Application.UseCases.Orders.Queries.GetOrderById;
using OrderService.Application.UseCases.Orders.Queries.GetOrdersByStatus;

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

        [HttpGet("client/status-{status}")]
        // client
        public async Task<IActionResult> GetClientOrdersByStatus(Guid clientId, string status, CancellationToken cancellationToken)
        {
            // client id will be retrieved from token

            var orders = await _mediator.Send(new GetClientOrdersByStatusQuery(clientId, status), cancellationToken);

            return Ok(orders);
        }

        [HttpPatch("{orderId:guid}/client-confirmation")]
        // client
        public async Task<IActionResult> ConfirmOrderByClient(Guid orderId, CancellationToken cancellationToken)
        {
            await _mediator.Send(new ConfirmOrderByClientCommand(orderId), cancellationToken);

            return NoContent();
        }

        [HttpGet("status-{status}")]
        // admin
        public async Task<IActionResult> GetOrdersByStatus(string status, CancellationToken cancellationToken)
        {
            var orders = await _mediator.Send(new GetOrdersByStatusQuery(status), cancellationToken);

            return Ok(orders);
        }

        [HttpPatch("{orderId:guid}/status/ready")]
        // admin
        public async Task<IActionResult> UpdateOrderWithReadyStatus(Guid orderId, CancellationToken cancellationToken)
        {
            await _mediator.Send(new UpdateOrderWithReadyStatusCommand(orderId), cancellationToken);

            return NoContent();
        }

        [HttpPatch("{courierId:guid}/status/out-for-delivery")]
        // admin
        public async Task<IActionResult> UpdateOrdersWithOutForDeliveryStatus(Guid courierId, CancellationToken cancellationToken)
        {
            await _mediator.Send(new UpdateOrdersWithOutForDeliveryStatusCommand(courierId), cancellationToken);

            return NoContent();
        }

        [HttpGet("opened-orders")]
        // courier
        public async Task<IActionResult> GetOpenedOrders(CancellationToken cancellationToken)
        {
            var orders = await _mediator.Send(new GetOpenedOrdersQuery(), cancellationToken);

            return Ok(orders);
        }

        [HttpPatch("{orderId:guid}/assign-courier")]
        // courier
        public async Task<IActionResult> UpdateOrderWithCourierId(Guid orderId, Guid courierId, CancellationToken cancellationToken)
        {
            // courierId will be retrieved from the token

            await _mediator.Send(new UpdateOrderWithCourierIdCommand(courierId, orderId), cancellationToken);

            return NoContent();
        }

        [HttpGet("out-for-delivery")]
        // courier
        public async Task<IActionResult> GetCourierOrders(Guid courierId, CancellationToken cancellationToken)
        {
            // courierId will be retrieved from the token

            var orders = await _mediator.Send(new GetCourierOrdersQuery(courierId), cancellationToken);

            return Ok(orders);
        }

        [HttpPatch("{orderId:guid}/courier-confirmation")]
        // courier
        public async Task<IActionResult> ConfirmOrderByCourier(Guid orderId, CancellationToken cancellationToken)
        {
            await _mediator.Send(new ConfirmOrderByCourierCommand(orderId), cancellationToken);

            return NoContent();
        }
    }
}
