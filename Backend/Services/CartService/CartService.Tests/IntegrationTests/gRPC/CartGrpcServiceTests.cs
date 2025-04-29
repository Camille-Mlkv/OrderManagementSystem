using CartService.Application.Specifications;
using CartService.Application.Specifications.Jobs;
using CartService.Domain.Entities;
using CartService.GrpcServer;
using CartService.Tests.IntegrationTests.Configuration;
using Grpc.Core;
using Grpc.Net.Client;
using Hangfire;
using Microsoft.Extensions.DependencyInjection;

namespace CartService.Tests.IntegrationTests.gRPC
{
    public class CartGrpcServiceTests: IClassFixture<CustomWebApplicationFactory>
    {
        private readonly CustomWebApplicationFactory _factory;
        private readonly GrpcChannel _channel;
        private readonly GrpcServer.CartService.CartServiceClient _client;

        private readonly IServiceScope _scope;
        private readonly IUnitOfWork _unitOfWork;
        private readonly Guid _testUserId;

        public CartGrpcServiceTests(CustomWebApplicationFactory factory)
        {
            _factory = factory;
            _scope = factory.Services.CreateScope();
            _unitOfWork = _scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
            _testUserId = Guid.NewGuid();

            var options = new GrpcChannelOptions
            {
                HttpHandler = factory.Server.CreateHandler()
            };
            _channel = GrpcChannel.ForAddress(factory.Server.BaseAddress, options);
            _client = new GrpcServer.CartService.CartServiceClient(_channel);
        }

        [Fact]
        public async Task GetCartContent_ShouldReturnContent()
        {
            // Arrange
            await SetupTestCart(_testUserId);

            var request = new GetCartByUserIdRequest { UserId = _testUserId.ToString() };

            // Act
            var response = await _client.GetCartContentAsync(request);

            // Assert
            Assert.Equal(2, response.Items.Count);
            Assert.Equal(2, response.Items[1].Quantity);
        }

        [Fact]
        public async Task ClearCart_InvalidUserId_ThrowsRpcException()
        {
            // Arrange
            var request = new ClearCartRequest { UserId = "invalid-guid" };

            // Act and Assert
            var ex = await Assert.ThrowsAsync<RpcException>(() =>
                _client.ClearCartAsync(request).ResponseAsync);

            Assert.Equal(StatusCode.InvalidArgument, ex.StatusCode);
        }


        [Fact]
        public async Task ClearCart_ValidUserId_DeletesCartAndJob()
        {
            // Arrange
            await SetupTestCart(_testUserId);

            await SetupTestJob(_testUserId);

            var request = new ClearCartRequest { UserId = _testUserId.ToString() };

            // Act
            var response = await _client.ClearCartAsync(request);

            // Assert
            await AssertCartDeleted(_testUserId);
            await AssertJobDeleted(_testUserId);
        }

        private async Task SetupTestCart(Guid userId)
        {
            var cart = new Cart { UserId = userId };

            for (int i = 0; i < 2; i++)
            {
                cart.Items.Add(new CartItem { MealId = Guid.NewGuid(), Quantity = i + 1 });
            }

            await _unitOfWork.CartRepository.SaveCartAsync(cart, CancellationToken.None);
        }

        private async Task SetupTestJob(Guid userId)
        {
            using var scope = _factory.Services.CreateScope();
            var _backgroundJobClient = _scope.ServiceProvider.GetRequiredService<IBackgroundJobClient>();

            var jobId = _backgroundJobClient.Schedule<ICartJobService>(
               x => x.ExecuteJobAsync(userId),
               TimeSpan.FromSeconds(1));
        }

        private async Task AssertCartDeleted(Guid userId)
        {
            var cart = await _unitOfWork.CartRepository.GetCartAsync(userId, CancellationToken.None);
            Assert.Null(cart);
        }

        private async Task AssertJobDeleted(Guid userId)
        {
            var deletedJobId = await _unitOfWork.CartJobRepository.GetJobIdAsync(userId, CancellationToken.None);
            Assert.Null(deletedJobId);
        }
    }

}
