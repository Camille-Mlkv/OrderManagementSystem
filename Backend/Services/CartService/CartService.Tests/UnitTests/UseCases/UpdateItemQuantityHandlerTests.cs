using CartService.Application.Specifications.Jobs;
using CartService.Application.Specifications.Repositories;
using CartService.Application.Specifications;
using Moq;
using CartService.Application.DTOs;
using CartService.Application.Exceptions;
using CartService.Application.UseCases.Commands.UpdateItemQuantity;
using CartService.Domain.Entities;

namespace CartService.Tests.UnitTests.UseCases
{
    public class UpdateItemQuantityHandlerTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<ICartRepository> _cartRepoMock;
        private readonly Mock<ICartJobService> _jobServiceMock;

        public UpdateItemQuantityHandlerTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _cartRepoMock = new Mock<ICartRepository>();
            _jobServiceMock = new Mock<ICartJobService>();

            _unitOfWorkMock.Setup(u => u.CartRepository).Returns(_cartRepoMock.Object);
        }

        [Fact]
        public async Task Handle_Should_ThrowBadRequestException_WhenCartIsEmpty()
        {
            // Arrange
            var userId = Guid.Parse("11111111-1111-1111-1111-111111111111");
            var command = new UpdateItemQuantityCommand(userId, new CartItemDto
            {
                MealId = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                Quantity = 2
            });

            _cartRepoMock
               .Setup(repo => repo.GetCartAsync(userId, It.IsAny<CancellationToken>()))
               .ReturnsAsync(default(Cart));

            var handler = new UpdateItemQuantityHandler(_unitOfWorkMock.Object, _jobServiceMock.Object);

            // Act
            var exception = await Assert.ThrowsAsync<BadRequestException>(() => handler.Handle(command, CancellationToken.None));

            // Assert
            Assert.Equal("Cart is empty.", exception.Details);
        }

        [Fact]
        public async Task Handle_Should_ThrowNotFoundException_WhenItemNotFoundInCart()
        {
            // Arrange
            var userId = Guid.Parse("11111111-1111-1111-1111-111111111111");
            var command = new UpdateItemQuantityCommand(userId, new CartItemDto
            {
                MealId = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                Quantity = 2
            });

            var cart = new Cart
            {
                UserId = userId,
                Items = new List<CartItem> { new CartItem { MealId = Guid.Parse("33333333-3333-3333-3333-333333333333"), Quantity = 1 } }
            };

            _cartRepoMock
                .Setup(repo => repo.GetCartAsync(userId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(cart);

            var handler = new UpdateItemQuantityHandler(_unitOfWorkMock.Object, _jobServiceMock.Object);

            // Act
            var exception = await Assert.ThrowsAsync<NotFoundException>(() => handler.Handle(command, CancellationToken.None));

            // Assert
            Assert.Equal("Meal 22222222-2222-2222-2222-222222222222 not found in user 11111111-1111-1111-1111-111111111111 cart.", exception.Details);
        }

        [Fact]
        public async Task Handle_Should_UpdateItemQuantity_WhenItemExistsInCart()
        {
            // Arrange
            var userId = Guid.Parse("11111111-1111-1111-1111-111111111111");
            var mealId = Guid.Parse("22222222-2222-2222-2222-222222222222");
            var newQuantity = 3;

            var command = new UpdateItemQuantityCommand(userId, new CartItemDto
            {
                MealId = mealId,
                Quantity = newQuantity
            });

            var cart = new Cart
            {
                UserId = userId,
                Items = new List<CartItem> { new CartItem { MealId = mealId, Quantity = 1 } }
            };

            _cartRepoMock
               .Setup(repo => repo.GetCartAsync(userId, It.IsAny<CancellationToken>()))
               .ReturnsAsync(cart);

            _cartRepoMock
               .Setup(repo => repo.SaveCartAsync(It.IsAny<Cart>(), It.IsAny<CancellationToken>()))
               .Returns(Task.CompletedTask);

            var handler = new UpdateItemQuantityHandler(_unitOfWorkMock.Object, _jobServiceMock.Object);

            // Act
            await handler.Handle(command, CancellationToken.None);

            // Assert
            var updatedItem = cart.Items.First(i => i.MealId == mealId);
            Assert.Equal(newQuantity, updatedItem.Quantity);

            _cartRepoMock.Verify(repo => repo.SaveCartAsync(cart, It.IsAny<CancellationToken>()), Times.Once); 
            _jobServiceMock.Verify(job => job.DeleteJobAsync(userId, It.IsAny<CancellationToken>()), Times.Once);
            _jobServiceMock.Verify(job => job.ScheduleJobAsync(userId, It.IsAny<CancellationToken>()), Times.Once);
        }

    }
}
