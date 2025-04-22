using CartService.Application.Specifications.Jobs;
using CartService.Application.Specifications.Repositories;
using CartService.Application.Specifications;
using CartService.Application.UseCases.Commands.ClearCart;
using Moq;

namespace CartService.Tests.UnitTests.UseCases
{
    public class ClearCartHandlerTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<ICartRepository> _cartRepoMock;
        private readonly Mock<ICartJobService> _jobServiceMock;

        public ClearCartHandlerTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _cartRepoMock = new Mock<ICartRepository>();
            _jobServiceMock = new Mock<ICartJobService>();

            _unitOfWorkMock.Setup(u => u.CartRepository).Returns(_cartRepoMock.Object);
        }

        [Fact]
        public async Task Handle_Should_DeleteCartAndJob()
        {
            // Arrange
            var userId = Guid.Parse("11111111-1111-1111-1111-111111111111");

            var command = new ClearCartCommand(userId);

            _cartRepoMock
               .Setup(repo => repo.DeleteCartAsync(userId, It.IsAny<CancellationToken>()))
               .Returns(Task.CompletedTask);

            _jobServiceMock
               .Setup(job => job.DeleteJobAsync(userId, It.IsAny<CancellationToken>()))
               .Returns(Task.CompletedTask);

            var handler = new ClearCartHandler(_unitOfWorkMock.Object, _jobServiceMock.Object);

            // Act
            await handler.Handle(command, CancellationToken.None);

            // Assert
            _cartRepoMock.Verify(repo => repo.DeleteCartAsync(userId, It.IsAny<CancellationToken>()), Times.Once);
            _jobServiceMock.Verify(job => job.DeleteJobAsync(userId, It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
