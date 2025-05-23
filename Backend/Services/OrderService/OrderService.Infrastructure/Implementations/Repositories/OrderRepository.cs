﻿using MongoDB.Driver;
using MongoDB.Driver.Linq;
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

        public async Task<Order> GetByIdAsync(Guid orderId, CancellationToken cancellationToken)
        {
            return await _ordersCollection.Find(o => o.Id == orderId).FirstOrDefaultAsync();
        }

        public async Task<List<Order>> GetListAsync(Expression<Func<Order, bool>> filter, CancellationToken cancellationToken)
        {
            return await _ordersCollection.Find(filter).ToListAsync(cancellationToken);
        }

        public async Task CreateAsync(Order order, CancellationToken cancellationToken)
        {
            await _ordersCollection.InsertOneAsync(order);
        }

        public async Task UpdateAsync(Order order, CancellationToken cancellationToken)
        {
            await _ordersCollection.ReplaceOneAsync(o => o.Id == order.Id, order);
        }

        public async Task DeleteAsync(Guid orderId, CancellationToken cancellationToken)
        {
            await _ordersCollection.DeleteOneAsync(o => o.Id == orderId);
        }
    }
}
