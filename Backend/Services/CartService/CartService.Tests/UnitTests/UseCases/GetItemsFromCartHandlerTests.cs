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

        public GetItemsFromCartHandlerTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _cartRepoMock = new Mock<ICartRepository>();
            _jobServiceMock = new Mock<ICartJobService>();
            _mapperMock = new Mock<IMapper>();

            _unitOfWorkMock.Setup(u => u.CartRepository).Returns(_cartRepoMock.Object);
        }

        [Fact]
        public async Task Handle_Should_ReturnCartItems_WhenCartHasItems()
        {
            // Arrange
            var userId = Guid.Parse("11111111-1111-1111-1111-111111111111");
            var mealId = Guid.Parse("22222222-2222-2222-2222-222222222222");

            var query = new GetItemsFromCartQuery(userId);
            var cart = new Cart
            {
                UserId = userId,
                Items = new List<CartItem>
                {
                    new CartItem { MealId = mealId, Quantity = 2 }
                }
            };

            _cartRepoMock
               .Setup(repo => repo.GetCartAsync(userId, It.IsAny<CancellationToken>()))
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
            var userId = Guid.Parse("11111111-1111-1111-1111-111111111111");
            var query = new GetItemsFromCartQuery(userId);

            _cartRepoMock
               .Setup(repo => repo.GetCartAsync(userId, It.IsAny<CancellationToken>()))
               .ReturnsAsync(default(Cart));

            var handler = new GetItemsFromCartHandler(_unitOfWorkMock.Object, _mapperMock.Object);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.Empty(result);
        }
    }
}
