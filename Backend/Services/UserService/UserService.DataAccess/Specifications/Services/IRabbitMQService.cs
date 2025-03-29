using RabbitMQ.Client.Events;

namespace UserService.DataAccess.Specifications.Services
{
    public interface IRabbitMQService
    {
        Task ConsumeAsync(AsyncEventHandler<BasicDeliverEventArgs> eventHandler, CancellationToken cancellationToken);
    }
}
