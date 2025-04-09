using MealService.Application.DTOs;
using MealService.Domain.Entities;
using System.Linq.Expressions;

namespace MealService.Application.Specifications.Repositories
{
    public interface IRepository<T> where T : BaseEntity
    {
        Task<IReadOnlyList<T>> ListAllAsync(CancellationToken cancellationToken);

        Task<IReadOnlyList<T>> ListAsync(Expression<Func<T, bool>> filter,
            CancellationToken cancellationToken,
            params Expression<Func<T, object>>[]? includesProperties);

        Task AddAsync(T entity, CancellationToken cancellationToken);

        Task Delete(T entity);

        Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken,
            params Expression<Func<T, object>>[]? includesProperties);

        Task<PagedList<T>> GetPagedListAsync(int pageNumber, int pageSize, CancellationToken cancellationToken, Expression<Func<T, bool>>? filter = null);

        Task<bool> AnyAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken);

    }
}
