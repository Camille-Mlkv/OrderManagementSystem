using CartService.Application.Specifications.Jobs;
using CartService.Application.Specifications.Repositories;
using CartService.Application.Specifications;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using Moq;
using CartService.GrpcServer.Services;
using CartService.GrpcServer;

namespace CartService.Tests.UnitTests.gRPC
{
    public class CartServiceImplTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<ICartRepository> _mockCartRepository;
        private readonly Mock<ICartJobService> _mockJobService;
        private readonly Mock<ILogger<CartServiceImpl>> _mockLogger;
        private readonly CartServiceImpl _service;
        private readonly Mock<ServerCallContext> _mockServerCallContext;

        public CartServiceImplTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockCartRepository = new Mock<ICartRepository>();
            _mockUnitOfWork.Setup(u => u.CartRepository).Returns(_mockCartRepository.Object);

            _mockLogger = new Mock<ILogger<CartServiceImpl>>();
            _mockJobService = new Mock<ICartJobService>();

            _service = new CartServiceImpl(_mockUnitOfWork.Object, _mockLogger.Object, _mockJobService.Object);
            _mockServerCallContext = new Mock<ServerCallContext>();
        }

        [Fact]
        public async Task GetCartContent_InvalidUserId_ThrowsRpcException()
        {
            // Arrange
            var request = new GetCartByUserIdRequest { UserId = "not-a-guid" };

            // Act and Assert
            var ex = await Assert.ThrowsAsync<RpcException>(() => _service.GetCartContent(request, _mockServerCallContext.Object));

            Assert.Equal(StatusCode.InvalidArgument, ex.StatusCode);
        }

        [Fact]
        public async Task GetCartContent_ValidUserId_ReturnsCartItems()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var request = new GetCartByUserIdRequest { UserId = userId.ToString() };

            var cart = new Domain.Entities.Cart
            {
                UserId = userId,
                Items = new List<Domain.Entities.CartItem>
                {
                    new Domain.Entities.CartItem
                    {
                        MealId = Guid.NewGuid(),
                        Quantity = 2
                    },
                    new Domain.Entities.CartItem
                    {
                        MealId = Guid.NewGuid(),
                        Quantity = 1
                    }
                }
            };

            _mockCartRepository
                .Setup(repo => repo.GetCartAsync(userId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(cart);

            // Act
            var result = await _service.GetCartContent(request, _mockServerCallContext.Object);

            // Assert
            Assert.Equal(2, result.Items.Count);
            Assert.Contains(result.Items, i => i.Quantity == 2);
            Assert.Contains(result.Items, i => i.Quantity == 1);
        }

        [Fact]
        public async Task ClearCart_ValidUserId_DeletesCartAndJob()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var request = new ClearCartRequest { UserId = userId.ToString() };

            _mockCartRepository
                .Setup(repo => repo.DeleteCartAsync(userId, It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);
            _mockJobService
                .Setup(job => job.DeleteJobAsync(userId, It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _service.ClearCart(request, _mockServerCallContext.Object);

            // Assert
            Assert.IsType<Empty>(result);
            _mockCartRepository.Verify(r => r.DeleteCartAsync(userId, It.IsAny<CancellationToken>()), Times.Once);
            _mockJobService.Verify(j => j.DeleteJobAsync(userId, It.IsAny<CancellationToken>()), Times.Once);
        }

    }
}
