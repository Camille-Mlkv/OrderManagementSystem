using CartService.Application.DTOs;
using CartService.Application.Exceptions;
using CartService.Application.UseCases.Commands.AddItemToCart;
using CartService.Application.UseCases.Commands.ClearCart;
using CartService.Application.UseCases.Commands.DeleteItemFromCart;
using CartService.Application.UseCases.Commands.UpdateItemQuantity;
using CartService.Application.UseCases.Queries.GetItemsFromCart;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CartService.API.Controllers
{
    [ApiController]
    [Route("api/cart")]
    public class CartController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<CartController> _logger;

        public CartController(IMediator mediator, IHttpContextAccessor httpContextAccessor, ILogger<CartController> logger)
        {
            _mediator = mediator;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetItemsFromCart(CancellationToken cancellationToken)
        {
            var userId = GetUserId();
            var items = await _mediator.Send(new GetItemsFromCartQuery(userId), cancellationToken);

            _logger.LogInformation($"Items retrieved from cart {userId}.");

            return Ok(items);
        }

        [HttpPost]
        public async Task<IActionResult> AddItemToCart([FromBody] CartItemRequestDto cartItem, CancellationToken cancellationToken)
        {
            var userId = GetUserId();
            await _mediator.Send(new AddItemToCartCommand(userId, cartItem),cancellationToken);

            _logger.LogInformation($"Item {cartItem.MealId} added to the cart {userId}.");

            return NoContent();
        }

        [HttpPut]
        public async Task<IActionResult> UpdateItemQuantity([FromBody] CartItemDto cartItem, CancellationToken cancellationToken)
        {
            var userId = GetUserId();
            await _mediator.Send(new UpdateItemQuantityCommand(userId, cartItem), cancellationToken);

            _logger.LogInformation($"Amount is updated for item {cartItem.MealId} in cart {userId}.");

            return NoContent();
        }

        [HttpDelete("{mealId:Guid}")]
        public async Task<IActionResult> DeleteItemFromCart([FromRoute] Guid mealId, CancellationToken cancellationToken)
        {
            var userId = GetUserId();
            await _mediator.Send(new DeleteItemFromCartCommand(userId, mealId), cancellationToken);

            _logger.LogInformation($"Item {mealId} deleted from cart {userId}.");

            return NoContent();
        }

        [HttpDelete]
        public async Task<IActionResult> ClearCart(CancellationToken cancellationToken)
        {
            var userId = GetUserId();
            await _mediator.Send(new ClearCartCommand(userId), cancellationToken);

            _logger.LogInformation($"Cart {userId} is cleared.");

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
