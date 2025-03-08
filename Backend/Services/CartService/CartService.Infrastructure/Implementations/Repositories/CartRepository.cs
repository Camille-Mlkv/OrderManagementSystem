using CartService.Application.Specifications.Repositories;
using CartService.Domain.Entities;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace CartService.Infrastructure.Implementations.Repositories
{
    public class CartRepository : ICartRepository
    {
        private readonly IConnectionMultiplexer _connectionMultiplexer;
        private readonly IDatabase _database;

        public CartRepository(IConnectionMultiplexer connectionMultiplexer)
        {
            _connectionMultiplexer = connectionMultiplexer;
            _database = _connectionMultiplexer.GetDatabase();
        }

        public async Task AddItemToCartAsync(string userId, CartItem cartItem)
        {
            var key = GetCartKey(userId);
            var cartItemJson = JsonConvert.SerializeObject(cartItem);

            await _database.ListRightPushAsync(key, cartItemJson);
        }

        public async Task<IEnumerable<CartItem>> GetCartItemsAsync(string userId)
        {
            var key = GetCartKey(userId);
            var cartItemsData = await _database.ListRangeAsync(key);

            var cartItems = cartItemsData
                .Select(data => JsonConvert.DeserializeObject<CartItem>(data!))
                .ToList();

            return cartItems!;
        }

        public async Task<CartItem?> GetItemFromCartAsync(string userId, Guid mealId)
        {
            var key = GetCartKey(userId);
            var cartItemsData = await _database.ListRangeAsync(key);

            var cartItem = cartItemsData
                .Select(data => JsonConvert.DeserializeObject<CartItem>(data!))
                .FirstOrDefault(item => item != null && item.MealId == mealId);

            return cartItem;
        }

        public async Task RemoveItemFromCartAsync(string userId, CartItem cartItem)
        {
            var key = GetCartKey(userId);
            var cartItemJson = JsonConvert.SerializeObject(cartItem);
            await _database.ListRemoveAsync(key, cartItemJson);
        }

        public async Task ClearCartAsync(string userId)
        {
            var key = GetCartKey(userId);
            await _database.KeyDeleteAsync(key);
        }

        private string GetCartKey(string userId)
        {
            return $"cart:{userId}";
        }
    }
}
