using CartService.Application.Specifications.Jobs;
using CartService.Application.Specifications;
using CartService.Domain.Entities;
using CartService.Tests.IntegrationTests.Configuration;
using Hangfire;
using Microsoft.Extensions.DependencyInjection;

namespace CartService.Tests.IntegrationTests.Services
{
    public class CartJobServiceIntegrationTests: IClassFixture<CustomWebApplicationFactory>
    {
        private readonly CustomWebApplicationFactory _factory;
        private readonly IServiceScope _scope;
        private readonly ICartJobService _cartJobService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IBackgroundJobClient _backgroundJobClient;

        public CartJobServiceIntegrationTests(CustomWebApplicationFactory factory)
        {
            _factory = factory;
            _scope = factory.Services.CreateScope();
            _cartJobService = _scope.ServiceProvider.GetRequiredService<ICartJobService>();
            _unitOfWork = _scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
            _backgroundJobClient = _scope.ServiceProvider.GetRequiredService<IBackgroundJobClient>();
        }

        [Fact]
        public async Task ScheduleJobAsync_ShouldCreateJobAndSaveId()
        {
            // Arrange
            var userId = Guid.NewGuid();

            // Act
            await _cartJobService.ScheduleJobAsync(userId, CancellationToken.None);

            // Assert
            var jobId = await _unitOfWork.CartJobRepository.GetJobIdAsync(userId, CancellationToken.None);
            Assert.NotNull(jobId);
            Assert.False(string.IsNullOrEmpty(jobId));

            var job = JobStorage.Current.GetMonitoringApi().JobDetails(jobId);
            Assert.NotNull(job);
        }

        [Fact]
        public async Task ExecuteJobAsync_ShouldDeleteCartAndJob()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var cart = new Cart
            {
                UserId = userId,
                Items = new List<CartItem>
                {
                    new CartItem
                    {
                        MealId = Guid.NewGuid(),
                        Quantity = 2
                    }
                }
            };
            await _unitOfWork.CartRepository.SaveCartAsync(cart, CancellationToken.None);

            var jobId = _backgroundJobClient.Schedule<ICartJobService>(
                x => x.ExecuteJobAsync(userId),
                TimeSpan.FromSeconds(1));

            await _unitOfWork.CartJobRepository.SaveJobIdAsync(userId, jobId, CancellationToken.None);

            await Task.Delay(TimeSpan.FromSeconds(30));

            // Assert
            var deletedCart = await _unitOfWork.CartRepository.GetCartAsync(userId, CancellationToken.None);
            Assert.Null(deletedCart);

            var deletedJobId = await _unitOfWork.CartJobRepository.GetJobIdAsync(userId, CancellationToken.None);
            Assert.Null(deletedJobId);
        }
    }
}
