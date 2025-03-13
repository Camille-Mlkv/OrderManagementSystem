using CartService.Domain.Entities;

namespace CartService.Application.Specifications.Repositories
{
    public interface ICartRepository
    {
        Task<Cart?> GetCartAsync(string userId, CancellationToken cancellationToken);

        Task SaveCartAsync(Cart cart, CancellationToken cancellationToken);

        Task DeleteCartAsync(string userId, CancellationToken cancellationToken);
    }
}
