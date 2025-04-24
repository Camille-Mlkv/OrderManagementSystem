using Hangfire;
using System.Linq.Expressions;
using CartService.Application.Specifications.Jobs;

namespace CartService.Infrastructure.Implementations.Hangfire
{
    public class CartJobClient : ICartJobClient
    {
        public string Schedule(Expression<Func<Task>> methodCall, TimeSpan delay)
        {
            return BackgroundJob.Schedule(methodCall, delay);
        }

        public bool Delete(string jobId)
        {
            return BackgroundJob.Delete(jobId);
        }
    }
}
