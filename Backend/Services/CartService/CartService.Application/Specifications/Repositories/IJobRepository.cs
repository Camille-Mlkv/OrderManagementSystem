namespace CartService.Application.Specifications.Repositories
{
    public interface IJobRepository
    {
        Task SaveJobIdAsync(Guid userId, string jobId, CancellationToken cancellationToken);

        Task DeleteJobIdAsync(Guid userId, CancellationToken cancellationToken);

        Task<string?> GetJobIdAsync(Guid userId, CancellationToken cancellationToken);
    }
}
