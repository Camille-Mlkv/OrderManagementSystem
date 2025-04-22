using CartService.Application.Specifications.Jobs;
using CartService.Application.Specifications.Repositories;
using CartService.Application.Specifications;
using CartService.Application.UseCases.Commands.DeleteItemFromCart;
using CartService.Domain.Entities;
using Moq;

namespace CartService.Tests.UnitTests.UseCases
{
    public class DeleteItemFromCartHandlerTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<ICartRepository> _cartRepoMock;
        private readonly Mock<ICartJobService> _jobServiceMock;

        public DeleteItemFromCartHandlerTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _cartRepoMock = new Mock<ICartRepository>();
            _jobServiceMock = new Mock<ICartJobService>();

            _unitOfWorkMock.Setup(u => u.CartRepository).Returns(_cartRepoMock.Object);
        }

        [Fact]
        public async Task Handle_Should_RemoveItemAndSaveCart_WhenItemExists()
        {
            // Arrange
            var userId = Guid.Parse("11111111-1111-1111-1111-111111111111");
            var mealId = Guid.Parse("22222222-2222-2222-2222-222222222222");

            var command = new DeleteItemFromCartCommand(userId, mealId);

            var cart = new Cart
            {
                UserId = userId,
                Items = new List<CartItem>
                {
                    new CartItem { MealId = mealId, Quantity = 1 }
                }
            };

            _cartRepoMock
               .Setup(repo => repo.GetCartAsync(userId, It.IsAny<CancellationToken>()))
               .ReturnsAsync(cart);

            _jobServiceMock
               .Setup(job => job.DeleteJobAsync(userId, It.IsAny<CancellationToken>()))
               .Returns(Task.CompletedTask);

            var handler = new DeleteItemFromCartHandler(_unitOfWorkMock.Object, _jobServiceMock.Object);

            // Act
            await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.DoesNotContain(cart.Items, item => item.MealId == mealId);

            _jobServiceMock.Verify(job => job.DeleteJobAsync(userId, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_Should_RemoveItemAndSaveCart_WhenOtherItemsExist()
        {
            // Arrange
            var userId = Guid.Parse("11111111-1111-1111-1111-111111111111");
            var mealIdToDelete = Guid.Parse("22222222-2222-2222-2222-222222222222");
            var otherMealId = Guid.Parse("33333333-3333-3333-3333-333333333333");

            var command = new DeleteItemFromCartCommand(userId, mealIdToDelete);

            var cart = new Cart
            {
                UserId = userId,
                Items = new List<CartItem>
                {
                    new CartItem { MealId = mealIdToDelete, Quantity = 1 },
                    new CartItem { MealId = otherMealId, Quantity = 2 }
                }
            };

            _cartRepoMock
               .Setup(repo => repo.GetCartAsync(userId, It.IsAny<CancellationToken>()))
               .ReturnsAsync(cart);

            _jobServiceMock
               .Setup(job => job.DeleteJobAsync(userId, It.IsAny<CancellationToken>()))
               .Returns(Task.CompletedTask);

            _jobServiceMock
               .Setup(job => job.ScheduleJobAsync(userId, It.IsAny<CancellationToken>()))
               .Returns(Task.CompletedTask);

            var handler = new DeleteItemFromCartHandler(_unitOfWorkMock.Object, _jobServiceMock.Object);

            // Act
            await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.DoesNotContain(cart.Items, item => item.MealId == mealIdToDelete);
            Assert.Contains(cart.Items, item => item.MealId == otherMealId);

            _cartRepoMock.Verify(repo => repo.SaveCartAsync(
                It.Is<Cart>(c => c.UserId == userId && c.Items.Count == 1 && c.Items[0].MealId == otherMealId),
                It.IsAny<CancellationToken>()), Times.Once);

            _jobServiceMock.Verify(job => job.DeleteJobAsync(userId, It.IsAny<CancellationToken>()), Times.Once);
            _jobServiceMock.Verify(job => job.ScheduleJobAsync(userId, It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
