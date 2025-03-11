namespace CartService.Application.Specifications.Jobs
{
    public interface ICartJobService
    {
        Task ScheduleJobAsync(string userId, CancellationToken cancellationToken);

        Task ExecuteJobAsync(string userId);

        Task DeleteJobAsync(string userId, CancellationToken cancellationToken);
    }
}
