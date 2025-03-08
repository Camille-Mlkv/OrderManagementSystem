using CartService.Application.DTOs;
using CartService.Application.UseCases.Commands.AddItemToCart;
using CartService.Application.UseCases.Commands.ClearCart;
using CartService.Application.UseCases.Commands.DecreaseItemQuantity;
using CartService.Application.UseCases.Commands.DeleteItemFromCart;
using CartService.Application.UseCases.Commands.IncreaseItemQuantity;
using CartService.Application.UseCases.Queries.GetItemByIdFromCart;
using CartService.Application.UseCases.Queries.GetItemsFromCart;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CartService.API.Controllers
{
    [ApiController]
    [Route("api/cart")]
    public class CartController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CartController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{userId}/items")]
        public async Task<IActionResult> GetItemsFromCart([FromRoute] string userId, CancellationToken cancellationToken)
        {
            var items = await _mediator.Send(new GetItemsFromCartQuery(userId));

            return Ok(items);
        }

        [HttpGet("{userId}/items/{mealId}")]
        public async Task<IActionResult> GetItemByIdFromCart([FromRoute] string userId, [FromRoute] Guid mealId, CancellationToken cancellationToken)
        {
            var item = await _mediator.Send(new GetItemByIdFromCartQuery(userId, mealId));

            return Ok(item);
        }

        [HttpPost("{userId}")]
        public async Task<IActionResult> AddItemToCart([FromBody] CartItemDto cartItem, [FromRoute] string userId, CancellationToken cancellationToken)
        {
            await _mediator.Send(new AddItemToCartCommand(userId, cartItem),cancellationToken);

            return NoContent();
        }

        [HttpPost("{userId}/items/{mealId}/increase")]
        public async Task<IActionResult> IncreaseItemQuantity([FromRoute] string userId, [FromRoute] Guid mealId, CancellationToken cancellationToken)
        {
            await _mediator.Send(new IncreaseItemQuantityCommand(userId, mealId), cancellationToken);
            return NoContent();
        }

        [HttpPost("{userId}/items/{mealId}/decrease")]
        public async Task<IActionResult> DecreaseItemQuantity([FromRoute] string userId, [FromRoute] Guid mealId, CancellationToken cancellationToken)
        {
            await _mediator.Send(new DecreaseItemQuantityCommand(userId, mealId), cancellationToken);
            return NoContent();
        }

        [HttpDelete("{userId}/items/{mealId}")]
        public async Task<IActionResult> DeleteItemFromCart([FromRoute] string userId, [FromRoute] Guid mealId, CancellationToken cancellationToken)
        {
            await _mediator.Send(new DeleteItemFromCartCommand(userId, mealId), cancellationToken);

            return NoContent();
        }

        [HttpDelete("{userId}/clear")]
        public async Task<IActionResult> CLearCart([FromRoute] string userId, CancellationToken cancellationToken)
        {
            await _mediator.Send(new ClearCartCommand(userId), cancellationToken);

            return NoContent();
        }
    }
}
