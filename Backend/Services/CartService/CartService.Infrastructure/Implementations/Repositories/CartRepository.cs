using CartService.Application.Specifications.Repositories;
using CartService.Domain.Entities;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace CartService.Infrastructure.Implementations.Repositories
{
    public class CartRepository: ICartRepository
    {
        private readonly IDatabase _database;

        public CartRepository(IDatabase database)
        {
            _database = database;
        }

        public CartRepository(IConnectionMultiplexer connectionMultiplexer)
        {
            _database = connectionMultiplexer.GetDatabase();
        }

        public async Task<Cart?> GetCartAsync(Guid userId, CancellationToken cancellationToken)
        {
            var key = GetCartKey(userId);

            var cartJson = await _database.StringGetAsync(key).WaitAsync(cancellationToken);

            return string.IsNullOrEmpty(cartJson) ? null : JsonConvert.DeserializeObject<Cart>(cartJson!);
        }

        public async Task SaveCartAsync(Cart cart, CancellationToken cancellationToken)
        {
            var key = GetCartKey(cart.UserId!.Value);

            var cartJson = JsonConvert.SerializeObject(cart);

            await _database.StringSetAsync(key, cartJson).WaitAsync(cancellationToken);
        }

        public async Task DeleteCartAsync(Guid userId, CancellationToken cancellationToken)
        {
            await _database.KeyDeleteAsync(GetCartKey(userId)).WaitAsync(cancellationToken);
        }

        private string GetCartKey(Guid userId) => 
           $"cart_{userId}";
    }
}
