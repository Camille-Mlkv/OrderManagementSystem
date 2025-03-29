using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using UserService.DataAccess.Specifications.Services;
using UserService.DataAccess.Options;
using RabbitMQ.Client.Events;

namespace UserService.DataAccess.Implementations.Services.RabbitMQ
{
    public class RabbitMQService : IRabbitMQService, IAsyncDisposable
    {
        private readonly ConnectionFactory _factory;
        private readonly IConnection _connection;
        private readonly IChannel _channel;
        private readonly string _queueName;

        public RabbitMQService(IOptions<RabbitMQOptions> options)
        {
            var settings = options.Value;

            _factory = new ConnectionFactory
            {
                HostName = settings.HostName,
                Port = settings.Port,
                UserName = settings.UserName,
                Password = settings.Password
            };

            _connection = _factory.CreateConnectionAsync().Result;
            _channel = _connection.CreateChannelAsync().Result;
            _queueName = options.Value.QueueName;
        }

        public async Task ConsumeAsync(AsyncEventHandler<BasicDeliverEventArgs> eventHandler, CancellationToken cancellationToken)
        {
            await _channel.QueueDeclareAsync(queue: _queueName, durable: true, exclusive: false, autoDelete: false);

            var consumer = new AsyncEventingBasicConsumer(_channel);

            consumer.ReceivedAsync += eventHandler;

            await _channel.BasicConsumeAsync(queue: _queueName, autoAck: true, consumer: consumer, cancellationToken);
        }

        public async ValueTask DisposeAsync()
        {
            if (_connection is { IsOpen: true })
            {
                await _connection.CloseAsync();
                _connection.Dispose();
                GC.SuppressFinalize(this);
            }
        }
    }
}
