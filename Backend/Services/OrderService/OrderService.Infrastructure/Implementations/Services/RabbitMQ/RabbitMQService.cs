using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OrderService.Application.DTOs.Order;
using OrderService.Application.Specifications.Services;
using OrderService.Infrastructure.Options;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace OrderService.Infrastructure.Implementations.Services.RabbitMQ
{
    public class RabbitMQService : IMessageService, IAsyncDisposable
    {
        private readonly ConnectionFactory _factory;
        private readonly IConnection _connection;
        private readonly IChannel _channel;
        private readonly ILogger<RabbitMQService> _logger;
        private readonly string _queueName;

        public RabbitMQService(IOptions<RabbitMQOptions> options, ILogger<RabbitMQService> logger)
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
            _logger = logger;
            _queueName = options.Value.QueueName;
        }

        public async Task PublishAsync(OrderStatusDto orderStatusDto)
        {
            var message = JsonSerializer.Serialize(orderStatusDto);
            var body = Encoding.UTF8.GetBytes(message);

            await _channel.QueueDeclareAsync(queue: _queueName, durable: true, exclusive: false, autoDelete: false);

            await _channel.BasicPublishAsync(exchange: string.Empty, routingKey: _queueName, body: body);

            _logger.LogInformation($"Message sent to queue '{_queueName}': {message}");
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
