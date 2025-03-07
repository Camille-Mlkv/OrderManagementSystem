using CartService.Domain.Entities;

namespace CartService.Application.Specifications.Repositories
{
    public interface ICartRepository
    {
        Task AddItemToCartAsync(string userId, CartItem cartItem);

        Task<CartItem?> GetItemFromCartAsync(string userId, Guid mealId);

        Task<IEnumerable<CartItem>> GetCartItemsAsync(string userId);
    }
}
