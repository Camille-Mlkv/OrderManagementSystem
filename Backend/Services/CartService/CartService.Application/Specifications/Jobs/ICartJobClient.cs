using System.Linq.Expressions;

namespace CartService.Application.Specifications.Jobs
{
    public interface ICartJobClient
    {
        string Schedule(Expression<Func<Task>> methodCall, TimeSpan delay);
        bool Delete(string jobId);
    }
}
