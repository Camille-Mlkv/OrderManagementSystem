using CartService.API.Controllers;
using CartService.Application.DTOs;
using CartService.Application.UseCases.Commands.AddItemToCart;
using CartService.Application.UseCases.Commands.ClearCart;
using CartService.Application.UseCases.Commands.DeleteItemFromCart;
using CartService.Application.UseCases.Commands.UpdateItemQuantity;
using CartService.Application.UseCases.Queries.GetItemsFromCart;
using CartService.Tests.Utilities.Extensions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System.Security.Claims;

namespace CartService.Tests.UnitTests.Controllers
{
    public class CartControllerTests
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly Mock<IHttpContextAccessor> _httpContextAccessorMock;
        private readonly Mock<ILogger<CartController>> _loggerMock;
        private readonly CartController _controller;
        private readonly Guid _userId;

        public CartControllerTests()
        {
            _mediatorMock = new Mock<IMediator>();
            _httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            _loggerMock = new Mock<ILogger<CartController>>();
            _userId = Guid.NewGuid();

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, _userId.ToString())
            };
            var identity = new ClaimsIdentity(claims, "TestAuthType");
            var user = new ClaimsPrincipal(identity);
            var httpContext = new DefaultHttpContext { User = user };
            _httpContextAccessorMock.Setup(a => a.HttpContext).Returns(httpContext);

            _controller = new CartController(_mediatorMock.Object, _httpContextAccessorMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task GetItemsFromCart_ShouldReturnOkResult_WithCartItems()
        {
            // Arrange
            var cartItems = new List<CartItemDto>
            {
                new CartItemDto { MealId = Guid.NewGuid(), Quantity = 2}
            };

            _mediatorMock
               .Setup(m => m.Send(It.Is<GetItemsFromCartQuery>(q => q.UserId == _userId), It.IsAny<CancellationToken>()))
               .ReturnsAsync(cartItems);

            // Act
            var result = await _controller.GetItemsFromCart(CancellationToken.None);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedItems = Assert.IsAssignableFrom<List<CartItemDto>>(okResult.Value);
            Assert.Single(returnedItems);

            _loggerMock.VerifyLog(LogLevel.Information, $"Items retrieved from cart {_userId}.", Times.Once());
        }

        [Fact]
        public async Task AddItemToCart_ShouldSendCommand_AndReturnNoContent()
        {
            // Arrange
            var cartItem = new CartItemRequestDto
            {
                MealId = Guid.NewGuid(),
                Quantity = 2
            };

            _mediatorMock
               .Setup(m => m.Send(It.IsAny<AddItemToCartCommand>(), It.IsAny<CancellationToken>()))
               .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.AddItemToCart(cartItem, CancellationToken.None);

            // Assert
            var noContentResult = Assert.IsType<NoContentResult>(result);

            _mediatorMock.Verify(m => m.Send(
                It.Is<AddItemToCartCommand>(c =>
                    c.UserId == _userId &&
                    c.Item.MealId == cartItem.MealId &&
                    c.Item.Quantity == cartItem.Quantity),
                It.IsAny<CancellationToken>()),
                Times.Once);

            _loggerMock.VerifyLog(LogLevel.Information, $"Item {cartItem.MealId} added to the cart {_userId}.", Times.Once());
        }

        [Fact]
        public async Task UpdateItemQuantity_ShouldSendCommand_AndReturnNoContent()
        {
            // Arrange
            var cartItem = new CartItemDto
            {
                MealId = Guid.NewGuid(),
                Quantity = 5
            };

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<UpdateItemQuantityCommand>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.UpdateItemQuantity(cartItem, CancellationToken.None);

            // Assert
            var noContentResult = Assert.IsType<NoContentResult>(result);

            _mediatorMock.Verify(m => m.Send(
                It.Is<UpdateItemQuantityCommand>(c =>
                    c.UserId == _userId &&
                    c.Item.MealId == cartItem.MealId &&
                    c.Item.Quantity == cartItem.Quantity),
                It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task DeleteItemFromCart_ShouldSendCommand_AndReturnNoContent()
        {
            // Arrange
            var mealId = Guid.NewGuid();

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<DeleteItemFromCartCommand>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.DeleteItemFromCart(mealId, CancellationToken.None);

            // Assert
            var noContentResult = Assert.IsType<NoContentResult>(result);

            _mediatorMock.Verify(m => m.Send(
                It.Is<DeleteItemFromCartCommand>(c =>
                    c.UserId == _userId && c.MealId == mealId),
                It.IsAny<CancellationToken>()),
                Times.Once);

            _loggerMock.VerifyLog(LogLevel.Information, $"Item {mealId} deleted from cart {_userId}.", Times.Once());
        }

        [Fact]
        public async Task ClearCart_ShouldSendCommand_AndReturnNoContent()
        {
            // Arrange
            _mediatorMock
                .Setup(m => m.Send(It.IsAny<ClearCartCommand>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.ClearCart(CancellationToken.None);

            // Assert
            var noContentResult = Assert.IsType<NoContentResult>(result);

            _mediatorMock.Verify(m => m.Send(
                It.Is<ClearCartCommand>(c => c.UserId == _userId),
                It.IsAny<CancellationToken>()),
                Times.Once);

            _loggerMock.VerifyLog(LogLevel.Information, $"Cart {_userId} is cleared.", Times.Once());
        }
    }
}
