using CartService.Domain.Entities;

namespace CartService.Application.Specifications.Repositories
{
    public interface ICartRepository
    {
        Task<Cart?> GetCartAsync(Guid userId, CancellationToken cancellationToken);

        Task SaveCartAsync(Cart cart, CancellationToken cancellationToken);

        Task DeleteCartAsync(Guid userId, CancellationToken cancellationToken);
    }
}
