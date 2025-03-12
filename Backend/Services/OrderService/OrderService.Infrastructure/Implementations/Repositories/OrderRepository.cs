using MongoDB.Driver;
using OrderService.Application.Specifications.Repositories;
using OrderService.Domain.Entities;
using System.Linq.Expressions;

namespace OrderService.Infrastructure.Implementations.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly IMongoCollection<Order> _ordersCollection;

        public OrderRepository(IMongoDatabase database)
        {
            _ordersCollection = database.GetCollection<Order>("Orders");
        }

        public async Task<Order> GetByIdAsync(Guid orderId)
        {
            return await _ordersCollection.Find(o => o.Id == orderId).FirstOrDefaultAsync();
        }

        public async Task<List<Order>> GetListAsync(Expression<Func<Order, bool>> filter)
        {
            return await _ordersCollection.Find(filter).ToListAsync();
        }

        public async Task CreateAsync(Order order)
        {
            await _ordersCollection.InsertOneAsync(order);
        }

        public async Task UpdateAsync(Order order)
        {
            await _ordersCollection.ReplaceOneAsync(o => o.Id == order.Id, order);
        }

        public async Task DeleteAsync(Guid orderId)
        {
            await _ordersCollection.DeleteOneAsync(o => o.Id == orderId);
        }
    }
}
