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

        private string GetCartKey(string userId)
        {
            return $"cart:{userId}";
        }
    }
}
