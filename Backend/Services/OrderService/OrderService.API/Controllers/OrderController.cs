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
        private readonly ILogger<OrderController> _logger;  

        public OrderController(IMediator mediator, IHttpContextAccessor httpContextAccessor, ILogger<OrderController> logger)
        {
            _mediator = mediator;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }

        [HttpGet("some/data")]
        public IActionResult Get()
        {
            _logger.LogInformation("Test order service");
            return Ok();
        }

        [Authorize(Policy = "Client")]
        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] AddressDto address, CancellationToken cancellationToken)
        {
            var clientId = GetUserId();
            var orderId = await _mediator.Send(new CreateOrderCommand(clientId, address),cancellationToken);

            _logger.LogInformation($"Order {orderId} was created.");

            return Ok(orderId);
        }

        [Authorize(Policy = "Client")]
        [HttpDelete("{orderId}")]
        public async Task<IActionResult> DeletePendingOrder(Guid orderId, CancellationToken cancellationToken)
        {
            await _mediator.Send(new DeletePendingOrderCommand(orderId), cancellationToken);

            _logger.LogInformation($"Order {orderId} was deleted.");

            return NoContent();
        }

        [Authorize]
        [HttpGet("{orderId}")]
        public async Task<IActionResult> GetOrderById(Guid orderId, CancellationToken cancellationToken)
        {
            var order = await _mediator.Send(new GetOrderByIdQuery(orderId), cancellationToken);

            _logger.LogInformation($"Order {orderId} retrieved.");

            return Ok(order);
        }

        [Authorize(Policy = "Client")]
        [HttpGet("client/status-{status}")]
        public async Task<IActionResult> GetClientOrdersByStatus(string status, CancellationToken cancellationToken)
        {
            var clientId = GetUserId();
            var orders = await _mediator.Send(new GetClientOrdersByStatusQuery(clientId, status), cancellationToken);

            _logger.LogInformation($"client {clientId} orders with status {status} retrieved.");

            return Ok(orders);
        }

        [Authorize(Policy = "Client")]
        [HttpPatch("{orderId:guid}/client-confirmation")]
        public async Task<IActionResult> ConfirmOrderByClient(Guid orderId, CancellationToken cancellationToken)
        {
            await _mediator.Send(new ConfirmOrderByClientCommand(orderId), cancellationToken);

            _logger.LogInformation($"Order {orderId} confirmed by client.");

            return NoContent();
        }

        [Authorize(Policy = "Admin")]
        [HttpGet("status-{status}")]
        public async Task<IActionResult> GetOrdersByStatus(string status, CancellationToken cancellationToken)
        {
            var orders = await _mediator.Send(new GetOrdersByStatusQuery(status), cancellationToken);

            _logger.LogInformation($"Orders with status {status} retrieved.");

            return Ok(orders);
        }

        [Authorize(Policy = "Admin")]
        [HttpPatch("{orderId:guid}/status/ready")]
        public async Task<IActionResult> UpdateOrderWithReadyStatus(Guid orderId, CancellationToken cancellationToken)
        {
            await _mediator.Send(new UpdateOrderWithReadyStatusCommand(orderId), cancellationToken);

            _logger.LogInformation($"Order {orderId} status set to ready.");

            return NoContent();
        }

        [Authorize(Policy = "Admin")]
        [HttpPatch("{courierId:guid}/status/out-for-delivery")]
        public async Task<IActionResult> UpdateOrdersWithOutForDeliveryStatus(CancellationToken cancellationToken)
        {
            var courierId = GetUserId();
            await _mediator.Send(new UpdateOrdersWithOutForDeliveryStatusCommand(courierId), cancellationToken);

            _logger.LogInformation($"Courier {courierId} orders status set to out for delivery.");

            return NoContent();
        }

        [Authorize(Policy = "Courier")]
        [HttpGet("opened-orders")]
        public async Task<IActionResult> GetOpenedOrders(CancellationToken cancellationToken)
        {
            var orders = await _mediator.Send(new GetOpenedOrdersQuery(), cancellationToken);

            _logger.LogInformation($"Opened orders retrieved.");

            return Ok(orders);
        }

        [Authorize(Policy = "Courier")]
        [HttpPatch("{orderId:guid}/assign-courier")]
        public async Task<IActionResult> UpdateOrderWithCourierId(Guid orderId, CancellationToken cancellationToken)
        {
            var courierId = GetUserId();
            await _mediator.Send(new UpdateOrderWithCourierIdCommand(courierId, orderId), cancellationToken);

            _logger.LogInformation($"Order {orderId} taken by courier {courierId}.");

            return NoContent();
        }

        [Authorize(Policy = "Courier")]
        [HttpGet("out-for-delivery")]
        public async Task<IActionResult> GetCourierOrders(CancellationToken cancellationToken)
        {
            var courierId = GetUserId();
            var orders = await _mediator.Send(new GetCourierOrdersQuery(courierId), cancellationToken);

            _logger.LogInformation($"Courier {courierId} orders retrieved.");

            return Ok(orders);
        }

        [Authorize(Policy = "Courier")]
        [HttpPatch("{orderId:guid}/courier-confirmation")]
        public async Task<IActionResult> ConfirmOrderByCourier(Guid orderId, CancellationToken cancellationToken)
        {
            await _mediator.Send(new ConfirmOrderByCourierCommand(orderId), cancellationToken);

            _logger.LogInformation($"Order {orderId} confirmed by courier.");

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
