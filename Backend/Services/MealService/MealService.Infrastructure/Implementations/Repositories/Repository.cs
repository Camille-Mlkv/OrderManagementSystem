using MealService.Application.DTOs;
using MealService.Application.Specifications.Repositories;
using MealService.Domain.Entities;
using MealService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace MealService.Infrastructure.Implementations.Repositories
{
    public class Repository<T> : IRepository<T> where T : BaseEntity
    {
        protected readonly AppDbContext _context;
        public Repository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(T entity, CancellationToken cancellationToken)
        {
            await _context.Set<T>().AddAsync(entity, cancellationToken);
        }

        public Task Delete(T entity)
        {
            _context.Set<T>().Remove(entity);

            return Task.CompletedTask;
        }

        public async Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken, params Expression<Func<T, object>>[]? includesProperties)
        {
            var query = _context.Set<T>().AsQueryable();

            query = query.Where(entity => entity.Id == id);

            if (includesProperties != null && includesProperties.Any())
            {
                foreach (var property in includesProperties)
                {
                    query = query.Include(property);
                }
            }

            return await query.FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<PagedList<T>> GetPagedListAsync(int pageNumber, int pageSize, CancellationToken cancellationToken, Expression<Func<T, bool>>? filter = null)
        {
            var query = _context.Set<T>().AsQueryable();

            if (filter != null)
            {
                query = query.Where(filter);
            }

            var count = query.Count();

            var items = await query
                .OrderBy(entity => entity.Id)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            return new PagedList<T>
            {
                Items = items,
                TotalCount = count,
                CurrentPage = pageNumber,
                PageSize = pageSize
            };
        }

        public async Task<IReadOnlyList<T>> ListAllAsync(CancellationToken cancellationToken)
        {
            return await _context.Set<T>().ToListAsync(cancellationToken);
        }

        public async Task<IReadOnlyList<T>> ListAsync(Expression<Func<T, bool>> filter, CancellationToken cancellationToken, params Expression<Func<T, object>>[]? includesProperties)
        {
            var query = _context.Set<T>().AsQueryable();

            if (includesProperties?.Any() == true)
            {
                foreach (var included in includesProperties)
                {
                    query = query.Include(included);
                }
            }

            if (filter != null)
            {
                query = query.Where(filter);
            }

            return await query.ToListAsync(cancellationToken);
        }

        public async Task<bool> AnyAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken)
        {
            return await _context.Set<T>().AnyAsync(predicate, cancellationToken);
        }
    }
}
