using AutoMapper;
using CartService.Application.DTOs;
using CartService.Application.Exceptions;
using CartService.Application.Specifications;
using CartService.Application.Specifications.Jobs;
using CartService.Application.Specifications.Repositories;
using CartService.Application.UseCases.Commands.AddItemToCart;
using CartService.Domain.Entities;
using MealService.GrpcServer;
using Moq;
using static CartService.Tests.Utilities.TestGrpcUtilities;

namespace CartService.Tests.UnitTests.UseCases
{
    public class AddItemToCartHandlerTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<ICartRepository> _cartRepoMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<ICartJobService> _jobServiceMock;
        private readonly Mock<MealService.GrpcServer.MealService.MealServiceClient> _grpcClientMock;

        private readonly Guid _userId;
        private readonly Guid _mealId;

        public AddItemToCartHandlerTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _cartRepoMock = new Mock<ICartRepository>();
            _mapperMock = new Mock<IMapper>();
            _jobServiceMock = new Mock<ICartJobService>();
            _grpcClientMock = new Mock<MealService.GrpcServer.MealService.MealServiceClient>();
            _userId = Guid.NewGuid();
            _mealId = Guid.NewGuid();

            _unitOfWorkMock.Setup(u => u.CartRepository).Returns(_cartRepoMock.Object);
        }

        [Fact]
        public async Task Handle_Should_AddItemToCart_And_ScheduleJob()
        {
            // Arrange
            var command = new AddItemToCartCommand(
                _userId,
                new CartItemRequestDto
                {
                    MealId = _mealId,
                    Quantity = 1
                }
            );

            var mealReply = new GetMealByIdReply
            {
                MealId = _mealId.ToString(),
                Name = "Test Meal",
                Price = 10.0
            };

            _grpcClientMock
               .Setup(client => client.GetMealByIdAsync(
                   It.Is<GetMealByIdRequest>(r => r.MealId == _mealId.ToString()),
                   null,
                   null,
                   It.IsAny<CancellationToken>()))
               .Returns(CreateAsyncUnaryCall(mealReply));

            var existingCart = new Cart
            {
                UserId = _userId,
                Items = new List<CartItem>()
            };

            _cartRepoMock
               .Setup(repo => repo.GetCartAsync(_userId, It.IsAny<CancellationToken>()))
               .ReturnsAsync(existingCart);

            var mappedItem = new CartItem
            {
                MealId = _mealId,
                Quantity = 1
            };

            _mapperMock
               .Setup(m => m.Map<CartItem>(command.Item))
               .Returns(mappedItem);

            var handler = new AddItemToCartHandler(
                _unitOfWorkMock.Object,
                _mapperMock.Object,
                _jobServiceMock.Object,
                _grpcClientMock.Object
            );

            // Act
            await handler.Handle(command, CancellationToken.None);

            // Assert
            _cartRepoMock.Verify(r => r.SaveCartAsync(
               It.Is<Cart>(c => c.UserId == _userId && c.Items.Contains(mappedItem)),
               It.IsAny<CancellationToken>()), Times.Once);

            _jobServiceMock.Verify(j => j.DeleteJobAsync(_userId, It.IsAny<CancellationToken>()), Times.Once);
            _jobServiceMock.Verify(j => j.ScheduleJobAsync(_userId, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldIncreaseQuantity_WhenItemAlreadyExistsInCart()
        {
            // Arrange
            var command = new AddItemToCartCommand(
                _userId,
                new CartItemRequestDto
                {
                    MealId = _mealId,
                    Quantity = 2
                }
            );

            var mealReply = new GetMealByIdReply
            {
                MealId = _mealId.ToString(),
                Name = "Test Meal",
                Price = 10.0
            };

            _grpcClientMock
               .Setup(client => client.GetMealByIdAsync(
                   It.Is<GetMealByIdRequest>(r => r.MealId == _mealId.ToString()),
                   null,
                   null,
                   It.IsAny<CancellationToken>()))
               .Returns(CreateAsyncUnaryCall(mealReply));

            var existingCartItem = new CartItem
            {
                MealId = _mealId,
                Quantity = 3
            };

            var existingCart = new Cart
            {
                UserId = _userId,
                Items = new List<CartItem> { existingCartItem }
            };

            _cartRepoMock
               .Setup(repo => repo.GetCartAsync(_userId, It.IsAny<CancellationToken>()))
               .ReturnsAsync(existingCart);

            var mappedItem = new CartItem
            {
                MealId = _mealId,
                Quantity = 2
            };

            _mapperMock
               .Setup(m => m.Map<CartItem>(command.Item))
               .Returns(mappedItem);

            var handler = new AddItemToCartHandler(
                _unitOfWorkMock.Object,
                _mapperMock.Object,
                _jobServiceMock.Object,
                _grpcClientMock.Object
            );

            // Act
            await handler.Handle(command, CancellationToken.None);

            // Assert
            _cartRepoMock.Verify(r => r.SaveCartAsync(
               It.Is<Cart>(c =>
                   c.Items.Count == 1 &&
                   c.Items.First().MealId == _mealId &&
                   c.Items.First().Quantity == 5),
               It.IsAny<CancellationToken>()), Times.Once);

            _jobServiceMock.Verify(j => j.DeleteJobAsync(_userId, It.IsAny<CancellationToken>()), Times.Once);
            _jobServiceMock.Verify(j => j.ScheduleJobAsync(_userId, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldThrowNotFoundException_WhenMealNotFound()
        {
            // Arrange
            var command = new AddItemToCartCommand(
                _userId,
                new CartItemRequestDto
                {
                    MealId = _mealId,
                    Quantity = 1
                }
            );

            _grpcClientMock
               .Setup(client => client.GetMealByIdAsync(
                   It.Is<GetMealByIdRequest>(r => r.MealId == _mealId.ToString()),
                   null,
                   null,
                   It.IsAny<CancellationToken>()))
               .Returns(CreateAsyncUnaryCall<GetMealByIdReply>(null!));

            var handler = new AddItemToCartHandler(
                _unitOfWorkMock.Object,
                _mapperMock.Object,
                _jobServiceMock.Object,
                _grpcClientMock.Object
            );

            // Act
            var exception = await Assert.ThrowsAsync<NotFoundException>(() => handler.Handle(command, CancellationToken.None));

            // Assert
            Assert.Equal("Failed to add item to the cart.", exception.Message);
            Assert.Contains(_mealId.ToString(), exception.Details);
        }
    }

}
