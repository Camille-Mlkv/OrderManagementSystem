using OrderService.Domain.Entities;
using System.Linq.Expressions;

namespace OrderService.Application.Specifications.Repositories
{
    public interface IOrderRepository
    {
        Task<List<Order>> GetListAsync(Expression<Func<Order, bool>> filter);

        Task<Order> GetByIdAsync(Guid orderId);

        Task CreateAsync(Order order);

        Task UpdateAsync(Order order);

        Task DeleteAsync(Guid orderId);
    }
}
