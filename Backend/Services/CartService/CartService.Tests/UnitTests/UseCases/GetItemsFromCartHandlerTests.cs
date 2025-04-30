using CartService.Application.Specifications.Jobs;
using CartService.Application.Specifications.Repositories;
using CartService.Application.Specifications;
using Moq;
using CartService.Application.DTOs;
using CartService.Application.UseCases.Queries.GetItemsFromCart;
using CartService.Domain.Entities;
using AutoMapper;

namespace CartService.Tests.UnitTests.UseCases
{
    public class GetItemsFromCartHandlerTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<ICartRepository> _cartRepoMock;
        private readonly Mock<ICartJobService> _jobServiceMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Guid _userId;

        public GetItemsFromCartHandlerTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _cartRepoMock = new Mock<ICartRepository>();
            _jobServiceMock = new Mock<ICartJobService>();
            _mapperMock = new Mock<IMapper>();
            _userId = Guid.NewGuid();

            _unitOfWorkMock.Setup(u => u.CartRepository).Returns(_cartRepoMock.Object);
        }

        [Fact]
        public async Task Handle_Should_ReturnCartItems_WhenCartHasItems()
        {
            // Arrange
            var mealId = Guid.NewGuid();

            var query = new GetItemsFromCartQuery(_userId);
            var cart = new Cart
            {
                UserId = _userId,
                Items = new List<CartItem>
                {
                    new CartItem { MealId = mealId, Quantity = 2 }
                }
            };

            _cartRepoMock
               .Setup(repo => repo.GetCartAsync(_userId, It.IsAny<CancellationToken>()))
               .ReturnsAsync(cart);

            var cartItemDto = new CartItemDto { MealId = mealId, Quantity = 2 };

            _mapperMock
               .Setup(m => m.Map<List<CartItemDto>>(cart.Items))
               .Returns(new List<CartItemDto> { cartItemDto });

            var handler = new GetItemsFromCartHandler(_unitOfWorkMock.Object, _mapperMock.Object);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.Single(result);
            Assert.Equal(mealId, result[0].MealId);
            Assert.Equal(2, result[0].Quantity);
        }

        [Fact]
        public async Task Handle_Should_ReturnEmptyList_WhenCartNotFound()
        {
            // Arrange
            var query = new GetItemsFromCartQuery(_userId);

            _cartRepoMock
               .Setup(repo => repo.GetCartAsync(_userId, It.IsAny<CancellationToken>()))
               .ReturnsAsync(default(Cart));

            var handler = new GetItemsFromCartHandler(_unitOfWorkMock.Object, _mapperMock.Object);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.Empty(result);
        }
    }
}
