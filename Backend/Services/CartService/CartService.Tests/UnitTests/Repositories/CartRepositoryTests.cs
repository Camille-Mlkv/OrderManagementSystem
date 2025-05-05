using CartService.Domain.Entities;
using CartService.Infrastructure.Implementations.Repositories;
using Moq;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace CartService.Tests.UnitTests.Repositories
{
    public class CartRepositoryTests
    {
        private readonly Mock<IDatabase> _databaseMock;
        private readonly CartRepository _cartRepository;
        private readonly Guid _userId = Guid.NewGuid();

        public CartRepositoryTests()
        {
            _databaseMock = new Mock<IDatabase>();

            var connectionMultiplexerMock = new Mock<IConnectionMultiplexer>();
            connectionMultiplexerMock
               .Setup(x => x.GetDatabase(It.IsAny<int>(), It.IsAny<object>()))
               .Returns(_databaseMock.Object);

            _cartRepository = new CartRepository(connectionMultiplexerMock.Object);
        }

        [Fact]
        public async Task GetCartAsync_ShouldReturnCart_WhenCartExists()
        {
            // Arrange
            var cart = new Cart
            {
                UserId = _userId,
                Items = new List<CartItem> 
                {
                    new CartItem { MealId = Guid.NewGuid(), Quantity = 2 }
                }
            };
            var cartJson = JsonConvert.SerializeObject(cart);

            var expectedKey = $"cart_{_userId}";

            _databaseMock
                .Setup(db => db.StringGetAsync(
                    expectedKey,
                    It.IsAny<CommandFlags>()))
                .ReturnsAsync(cartJson);

            // Act
            var result = await _cartRepository.GetCartAsync(_userId, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(_userId, result.UserId);
            Assert.Single(result.Items);

            var cartItem = result.Items.First();
            Assert.Equal(2, cartItem.Quantity);
        }

        [Fact]
        public async Task GetCartAsync_ShouldReturnNull_WhenCartNotExists()
        {
            // Arrange
            var cart = new Cart();
           
            var cartJson = JsonConvert.SerializeObject(cart);

            var expectedKey = $"cart_{_userId}";

            _databaseMock
                .Setup(db => db.StringGetAsync(
                    expectedKey,
                    It.IsAny<CommandFlags>()))
                .ReturnsAsync("");

            // Act
            var result = await _cartRepository.GetCartAsync(_userId, CancellationToken.None);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task SaveCartAsync_ShouldStoreCartInRedis()
        {
            // Arrange
            var cart = new Cart
            {
                UserId = _userId,
                Items = new List<CartItem>
                {
                    new CartItem { MealId = Guid.NewGuid(), Quantity = 1 }
                }
            };
            var expectedKey = $"cart_{_userId}";
            var expectedValue = JsonConvert.SerializeObject(cart);

            _databaseMock
               .Setup(db => db.StringSetAsync(
                   expectedKey,
                   expectedValue,
                   null,
                   When.Always,
                   CommandFlags.None))
               .ReturnsAsync(true);

            // Act
            await _cartRepository.SaveCartAsync(cart, CancellationToken.None);

            // Assert
            _databaseMock.Verify(db =>
               db.StringSetAsync(
                  expectedKey,
                  expectedValue,
                  null,
                  It.IsAny<bool>(),
                  When.Always,
                  CommandFlags.None), Times.Once);
        }

        [Fact]
        public async Task DeleteCartAsync_ShouldRemoveCartFromRedis()
        {
            // Arrange
            var expectedKey = $"cart_{_userId}";

            _databaseMock
                .Setup(db => db.KeyDeleteAsync(expectedKey, CommandFlags.None))
                .ReturnsAsync(true);

            // Act
            await _cartRepository.DeleteCartAsync(_userId, CancellationToken.None);

            // Assert
            _databaseMock.Verify(db =>
                db.KeyDeleteAsync(expectedKey, CommandFlags.None), Times.Once);
        }

    }
}
