using CartService.Application.Specifications.Repositories;
using StackExchange.Redis;

namespace CartService.Infrastructure.Implementations.Repositories
{
    public class CartJobRepository: IJobRepository
    {
        private readonly IDatabase _database;
        private const string JobKeyPrefix = "cart_job_";

        public CartJobRepository(IConnectionMultiplexer connectionMultiplexer)
        {
            _database = connectionMultiplexer.GetDatabase();
        }

        public async Task SaveJobIdAsync(Guid userId, string jobId, CancellationToken cancellationToken)
        {
            await _database.StringSetAsync(GetJobKey(userId), jobId).WaitAsync(cancellationToken);
        }

        public async Task DeleteJobIdAsync(Guid userId, CancellationToken cancellationToken)
        {
            await _database.KeyDeleteAsync(GetJobKey(userId)).WaitAsync(cancellationToken);
        }

        public async Task<string?> GetJobIdAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            return await _database.StringGetAsync(GetJobKey(userId)).WaitAsync(cancellationToken);
        }

        private string GetJobKey(Guid userId) => 
           $"{JobKeyPrefix}{userId}";
    }
}
