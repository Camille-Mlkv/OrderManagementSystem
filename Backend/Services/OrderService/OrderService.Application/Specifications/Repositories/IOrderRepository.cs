using OrderService.Domain.Entities;
using System.Linq.Expressions;

namespace OrderService.Application.Specifications.Repositories
{
    public interface IOrderRepository
    {
        Task<List<Order>> GetListAsync(Expression<Func<Order, bool>> filter, CancellationToken cancellationToken);

        Task<Order> GetByIdAsync(Guid orderId, CancellationToken cancellationToken);

        Task CreateAsync(Order order, CancellationToken cancellationToken);

        Task UpdateAsync(Order order, CancellationToken cancellationToken);

        Task DeleteAsync(Guid orderId, CancellationToken cancellationToken);
    }
}
