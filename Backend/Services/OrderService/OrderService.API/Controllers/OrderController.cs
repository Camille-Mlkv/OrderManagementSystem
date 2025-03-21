using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrderService.Application.DTOs.Address;
using OrderService.Application.Exceptions;
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
using System.Security.Claims;

namespace OrderService.API.Controllers
{
    [ApiController]
    [Route("api/orders")]
    public class OrderController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public OrderController(IMediator mediator, IHttpContextAccessor httpContextAccessor)
        {
            _mediator = mediator;
            _httpContextAccessor = httpContextAccessor;
        }

        [Authorize(Policy = "Client")]
        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] AddressDto address, CancellationToken cancellationToken)
        {
            var clientId = GetUserId();

            var orderId = await _mediator.Send(new CreateOrderCommand(clientId, address),cancellationToken);

            return Ok(orderId);
        }

        [Authorize(Policy = "Client")]
        [HttpDelete("{orderId}")]
        public async Task<IActionResult> DeletePendingOrder(Guid orderId, CancellationToken cancellationToken)
        {
            await _mediator.Send(new DeletePendingOrderCommand(orderId), cancellationToken);

            return NoContent();
        }

        [Authorize]
        [HttpGet("{orderId}")]
        public async Task<IActionResult> GetOrderById(Guid orderId, CancellationToken cancellationToken)
        {
            var order = await _mediator.Send(new GetOrderByIdQuery(orderId), cancellationToken);

            return Ok(order);
        }

        [Authorize(Policy = "Client")]
        [HttpGet("client/status-{status}")]
        public async Task<IActionResult> GetClientOrdersByStatus(string status, CancellationToken cancellationToken)
        {
            var clientId = GetUserId();

            var orders = await _mediator.Send(new GetClientOrdersByStatusQuery(clientId, status), cancellationToken);

            return Ok(orders);
        }

        [Authorize(Policy = "Client")]
        [HttpPatch("{orderId:guid}/client-confirmation")]
        public async Task<IActionResult> ConfirmOrderByClient(Guid orderId, CancellationToken cancellationToken)
        {
            await _mediator.Send(new ConfirmOrderByClientCommand(orderId), cancellationToken);

            return NoContent();
        }

        [Authorize(Policy = "Admin")]
        [HttpGet("status-{status}")]
        public async Task<IActionResult> GetOrdersByStatus(string status, CancellationToken cancellationToken)
        {
            var orders = await _mediator.Send(new GetOrdersByStatusQuery(status), cancellationToken);

            return Ok(orders);
        }

        [Authorize(Policy = "Admin")]
        [HttpPatch("{orderId:guid}/status/ready")]
        public async Task<IActionResult> UpdateOrderWithReadyStatus(Guid orderId, CancellationToken cancellationToken)
        {
            await _mediator.Send(new UpdateOrderWithReadyStatusCommand(orderId), cancellationToken);

            return NoContent();
        }

        [Authorize(Policy = "Admin")]
        [HttpPatch("{courierId:guid}/status/out-for-delivery")]
        public async Task<IActionResult> UpdateOrdersWithOutForDeliveryStatus(CancellationToken cancellationToken)
        {
            var courierId = GetUserId();

            await _mediator.Send(new UpdateOrdersWithOutForDeliveryStatusCommand(courierId), cancellationToken);

            return NoContent();
        }

        [Authorize(Policy = "Courier")]
        [HttpGet("opened-orders")]
        public async Task<IActionResult> GetOpenedOrders(CancellationToken cancellationToken)
        {
            var orders = await _mediator.Send(new GetOpenedOrdersQuery(), cancellationToken);

            return Ok(orders);
        }

        [Authorize(Policy = "Courier")]
        [HttpPatch("{orderId:guid}/assign-courier")]
        public async Task<IActionResult> UpdateOrderWithCourierId(Guid orderId, CancellationToken cancellationToken)
        {
            var courierId = GetUserId();

            await _mediator.Send(new UpdateOrderWithCourierIdCommand(courierId, orderId), cancellationToken);

            return NoContent();
        }

        [Authorize(Policy = "Courier")]
        [HttpGet("out-for-delivery")]
        public async Task<IActionResult> GetCourierOrders(CancellationToken cancellationToken)
        {
            var courierId = GetUserId();

            var orders = await _mediator.Send(new GetCourierOrdersQuery(courierId), cancellationToken);

            return Ok(orders);
        }

        [Authorize(Policy = "Courier")]
        [HttpPatch("{orderId:guid}/courier-confirmation")]
        public async Task<IActionResult> ConfirmOrderByCourier(Guid orderId, CancellationToken cancellationToken)
        {
            await _mediator.Send(new ConfirmOrderByCourierCommand(orderId), cancellationToken);

            return NoContent();
        }

        private Guid GetUserId()
        {
            var userIdString = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value
               ?? throw new UnauthorizedException("Unauthorized.", "To access orders' api you must be logged in.");

            if (!Guid.TryParse(userIdString, out var userId))
            {
                throw new UnauthorizedException("Unauthorized.", "User ID is not a valid GUID.");
            }

            return userId;
        }
    }
}
