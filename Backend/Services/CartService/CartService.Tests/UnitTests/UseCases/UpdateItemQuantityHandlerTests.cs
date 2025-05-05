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
        private readonly Guid _userId;

        public UpdateItemQuantityHandlerTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _cartRepoMock = new Mock<ICartRepository>();
            _jobServiceMock = new Mock<ICartJobService>();
            _userId = Guid.NewGuid();

            _unitOfWorkMock.Setup(u => u.CartRepository).Returns(_cartRepoMock.Object);
        }

        [Fact]
        public async Task Handle_Should_ThrowBadRequestException_WhenCartIsEmpty()
        {
            // Arrange
            var command = new UpdateItemQuantityCommand(_userId, new CartItemDto
            {
                MealId = Guid.NewGuid(),
                Quantity = 2
            });

            _cartRepoMock
               .Setup(repo => repo.GetCartAsync(_userId, It.IsAny<CancellationToken>()))
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
            var mealId = Guid.NewGuid();
            var command = new UpdateItemQuantityCommand(_userId, new CartItemDto
            {
                MealId = mealId,
                Quantity = 2
            });

            var cart = new Cart
            {
                UserId = _userId,
                Items = new List<CartItem> { new CartItem { MealId = Guid.NewGuid(), Quantity = 1 } }
            };

            _cartRepoMock
                .Setup(repo => repo.GetCartAsync(_userId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(cart);

            var handler = new UpdateItemQuantityHandler(_unitOfWorkMock.Object, _jobServiceMock.Object);

            // Act
            var exception = await Assert.ThrowsAsync<NotFoundException>(() => handler.Handle(command, CancellationToken.None));

            // Assert
            Assert.Equal($"Meal {mealId} not found in user {_userId} cart.", exception.Details);
        }

        [Fact]
        public async Task Handle_Should_UpdateItemQuantity_WhenItemExistsInCart()
        {
            // Arrange
            var mealId = Guid.NewGuid();
            var newQuantity = 3;

            var command = new UpdateItemQuantityCommand(_userId, new CartItemDto
            {
                MealId = mealId,
                Quantity = newQuantity
            });

            var cart = new Cart
            {
                UserId = _userId,
                Items = new List<CartItem> { new CartItem { MealId = mealId, Quantity = 1 } }
            };

            _cartRepoMock
               .Setup(repo => repo.GetCartAsync(_userId, It.IsAny<CancellationToken>()))
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
            _jobServiceMock.Verify(job => job.DeleteJobAsync(_userId, It.IsAny<CancellationToken>()), Times.Once);
            _jobServiceMock.Verify(job => job.ScheduleJobAsync(_userId, It.IsAny<CancellationToken>()), Times.Once);
        }

    }
}
