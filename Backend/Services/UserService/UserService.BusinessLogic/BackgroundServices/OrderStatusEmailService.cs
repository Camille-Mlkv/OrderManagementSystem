using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;
using UserService.BusinessLogic.Specifications.Services;
using UserService.DataAccess.Specifications.Services;
using UserService.BusinessLogic.DTOs.Order;
using UserService.DataAccess.Specifications;
using Microsoft.Extensions.DependencyInjection;

namespace UserService.BusinessLogic.BackgroundServices
{
    public class OrderStatusEmailService : BackgroundService
    {
        private readonly IRabbitMQService _rabbitMQService;
        private readonly ILogger<OrderStatusEmailService> _logger;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public OrderStatusEmailService(
            IRabbitMQService rabbitMQService,
            IServiceScopeFactory serviceScopeFactory,
            ILogger<OrderStatusEmailService> logger)
        {
            _rabbitMQService = rabbitMQService;
            _serviceScopeFactory = serviceScopeFactory;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await _rabbitMQService.ConsumeAsync(HandleMessageAsync, stoppingToken);
        }

        private async Task HandleMessageAsync(object sender, BasicDeliverEventArgs eventArgs)
        {
            var body = eventArgs.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);

            _logger.LogInformation($"Received message: {message}");

            OrderStatusDto? orderStatusDto;
            try
            {
                orderStatusDto = JsonSerializer.Deserialize<OrderStatusDto>(message);

                if (orderStatusDto is null)
                {
                    _logger.LogWarning("Failed to deserialize message.");

                    return;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while deserializing message.");
                return;
            }

            using (var scope = _serviceScopeFactory.CreateScope())
            {
                _logger.LogInformation($"START SENDING EMAIL.");

                var emailService = scope.ServiceProvider.GetRequiredService<IEmailService>();
                var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

                var subject = $"Order {orderStatusDto.OrderNumber} status update";
                var emailBody = $"Dear customer, your order {orderStatusDto.OrderNumber} obtained new status: {orderStatusDto.OrderStatus}.";
                var emailAddress = await unitOfWork.UserRepository.GetUserEmailByIdAsync(orderStatusDto.UserId.ToString(), eventArgs.CancellationToken);

                await emailService.SendEmailAsync(emailAddress, subject, emailBody, eventArgs.CancellationToken);

                _logger.LogInformation($"Email sent to {emailAddress} for order {orderStatusDto.OrderNumber}.");
            }
        }
    }
}
