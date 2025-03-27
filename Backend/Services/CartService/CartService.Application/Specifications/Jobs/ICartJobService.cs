namespace CartService.Application.Specifications.Jobs
{
    public interface ICartJobService
    {
        Task ScheduleJobAsync(Guid userId, CancellationToken cancellationToken);

        Task ExecuteJobAsync(Guid userId);

        Task DeleteJobAsync(Guid userId, CancellationToken cancellationToken);
    }
}
