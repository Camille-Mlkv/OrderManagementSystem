using CartService.Application.DTOs;
using CartService.Application.Exceptions;
using CartService.Application.UseCases.Commands.AddItemToCart;
using CartService.Application.UseCases.Commands.ClearCart;
using CartService.Application.UseCases.Commands.DecreaseItemQuantity;
using CartService.Application.UseCases.Commands.DeleteItemFromCart;
using CartService.Application.UseCases.Commands.IncreaseItemQuantity;
using CartService.Application.UseCases.Queries.GetItemsFromCart;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CartService.API.Controllers
{
    [ApiController]
    [Route("api/cart")]
    [Authorize(Policy = "Client")]
    public class CartController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CartController(IMediator mediator, IHttpContextAccessor httpContextAccessor)
        {
            _mediator = mediator;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpGet]
        public async Task<IActionResult> GetItemsFromCart(CancellationToken cancellationToken)
        {
            var userId = GetUserId();
            var items = await _mediator.Send(new GetItemsFromCartQuery(userId), cancellationToken);

            return Ok(items);
        }

        [HttpPost]
        public async Task<IActionResult> AddItemToCart([FromBody] CartItemRequestDto cartItem, CancellationToken cancellationToken)
        {
            var userId = GetUserId();
            await _mediator.Send(new AddItemToCartCommand(userId, cartItem),cancellationToken);

            return NoContent();
        }

        [HttpPost("{mealId}/increase")]
        public async Task<IActionResult> IncreaseItemQuantity([FromRoute] Guid mealId, CancellationToken cancellationToken)
        {
            var userId = GetUserId();
            await _mediator.Send(new IncreaseItemQuantityCommand(userId, mealId), cancellationToken);

            return NoContent();
        }

        [HttpPost("{mealId}/decrease")]
        public async Task<IActionResult> DecreaseItemQuantity([FromRoute] Guid mealId, CancellationToken cancellationToken)
        {
            var userId = GetUserId();
            await _mediator.Send(new DecreaseItemQuantityCommand(userId, mealId), cancellationToken);

            return NoContent();
        }

        [HttpDelete("{mealId}")]
        public async Task<IActionResult> DeleteItemFromCart([FromRoute] Guid mealId, CancellationToken cancellationToken)
        {
            var userId = GetUserId();
            await _mediator.Send(new DeleteItemFromCartCommand(userId, mealId), cancellationToken);

            return NoContent();
        }

        [HttpDelete]
        public async Task<IActionResult> CLearCart(CancellationToken cancellationToken)
        {
            var userId = GetUserId();
            await _mediator.Send(new ClearCartCommand(userId), cancellationToken);

            return NoContent();
        }

        private string GetUserId()
        {
            return _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value
               ?? throw new UnauthorizedException("Unauthorized.", "Cart is available only to clients.");
        }
    }
}
