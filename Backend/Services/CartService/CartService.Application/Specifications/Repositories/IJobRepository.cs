namespace CartService.Application.Specifications.Repositories
{
    public interface IJobRepository
    {
        Task SaveJobIdAsync(string userId, string jobId, CancellationToken cancellationToken);

        Task DeleteJobIdAsync(string userId, CancellationToken cancellationToken);

        Task<string?> GetJobIdAsync(string userId, CancellationToken cancellationToken);
    }
}
