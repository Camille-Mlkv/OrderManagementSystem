using CartService.Application.Specifications.Repositories;
using CartService.Domain.Entities;
using CartService.Tests.IntegrationTests.Configuration;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;

namespace CartService.Tests.IntegrationTests.Repositories
{
    public class CartRepositoryIntegrationTests: IClassFixture<CustomWebApplicationFactory>
    {
        private readonly CustomWebApplicationFactory _factory;

        public CartRepositoryIntegrationTests(CustomWebApplicationFactory factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task SaveCartAsync_ShouldSaveCartInRedis()
        {
            // Arrange
            using var scope = _factory.Services.CreateScope();
            var repository = scope.ServiceProvider.GetRequiredService<ICartRepository>();

            var userId = Guid.NewGuid();
            var cart = new Cart
            {
                UserId = userId,
                Items = new List<CartItem>
                {
                    new CartItem
                    {
                        MealId = Guid.NewGuid(),
                        Quantity = 2
                    }
                }
            };

            // Act
            await repository.SaveCartAsync(cart, CancellationToken.None);

            // Assert
            var savedCart = await repository.GetCartAsync(userId, CancellationToken.None);
            savedCart.Should().NotBeNull();
            savedCart!.UserId.Should().Be(userId);
            savedCart.Items.Should().HaveCount(1);
        }

        [Fact]
        public async Task DeleteCartAsync_ShouldRemoveCartFromRedis()
        {
            // Arrange
            using var scope = _factory.Services.CreateScope();
            var repository = scope.ServiceProvider.GetRequiredService<ICartRepository>();

            var userId = Guid.NewGuid();
            var cart = new Cart
            {
                UserId = userId,
                Items = new List<CartItem>
                {
                    new CartItem
                    {
                        MealId = Guid.NewGuid(),
                        Quantity = 1
                    }
                }
            };

            await repository.SaveCartAsync(cart, CancellationToken.None);

            // Act
            await repository.DeleteCartAsync(userId, CancellationToken.None);

            // Assert
            var deletedCart = await repository.GetCartAsync(userId, CancellationToken.None);
            deletedCart.Should().BeNull();
        }
    }
}
