using CartService.Application.Specifications.Repositories;
using CartService.Application.Specifications;
using CartService.Infrastructure.Implementations.Hangfire;
using Moq;
using CartService.Application.Specifications.Jobs;
using System.Linq.Expressions;
using CartService.Domain.Entities;

namespace CartService.Tests.UnitTests.Hangfire
{
    public class CartJobServiceTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<ICartRepository> _cartRepositoryMock;
        private readonly Mock<IJobRepository> _jobRepositoryMock;
        private readonly Mock<ICartJobClient> _cartJobClientMock;
        private readonly CartJobService _cartJobService;
        private readonly Guid _userId = Guid.NewGuid();

        public CartJobServiceTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _cartRepositoryMock = new Mock<ICartRepository>();
            _jobRepositoryMock = new Mock<IJobRepository>();
            _cartJobClientMock = new Mock<ICartJobClient>();

            _unitOfWorkMock.SetupGet(u => u.CartRepository).Returns(_cartRepositoryMock.Object);
            _unitOfWorkMock.SetupGet(u => u.CartJobRepository).Returns(_jobRepositoryMock.Object);

            _cartJobService = new CartJobService(_unitOfWorkMock.Object, _cartJobClientMock.Object);
        }

        [Fact]
        public async Task ScheduleJobAsync_ShouldScheduleJob_WhenCalled()
        {
            // Arrange
            var expectedJobId = "job-123";
            var expectedDelay = TimeSpan.FromHours(3);

            _cartJobClientMock
                .Setup(client => client.Schedule(It.IsAny<Expression<Func<Task>>>(), expectedDelay))
                .Returns(expectedJobId);

            // Act
            await _cartJobService.ScheduleJobAsync(_userId, CancellationToken.None);

            // Assert
            _cartJobClientMock.Verify(client => client.Schedule(
                It.IsAny<Expression<Func<Task>>>(), expectedDelay), Times.Once);

            _unitOfWorkMock.Verify(uow => uow.CartJobRepository.SaveJobIdAsync(
                _userId, expectedJobId, CancellationToken.None), Times.Once);
        }

        [Fact]
        public async Task ExecuteJobAsync_ShouldDeleteCartAndJob_WhenCartExists()
        {
            // Arrange
            var cart = new Cart { UserId = _userId };

            _cartRepositoryMock
                .Setup(repo => repo.GetCartAsync(_userId, CancellationToken.None))
                .ReturnsAsync(cart);
            _jobRepositoryMock
                .Setup(repo => repo.GetJobIdAsync(_userId, CancellationToken.None))
                .ReturnsAsync("some-job-id");

            // Act
            await _cartJobService.ExecuteJobAsync(_userId);

            // Assert
            _cartRepositoryMock.Verify(repo => repo.DeleteCartAsync(_userId, CancellationToken.None), Times.Once);
            _jobRepositoryMock.Verify(repo => repo.GetJobIdAsync(_userId, CancellationToken.None), Times.Once);
            _cartJobClientMock.Verify(client => client.Delete(It.IsAny<string>()), Times.Once);
            _jobRepositoryMock.Verify(repo => repo.DeleteJobIdAsync(_userId, CancellationToken.None), Times.Once);
        }

        [Fact]
        public async Task ExecuteJobAsync_ShouldDoNothing_WhenCartDoesNotExist()
        {
            // Arrange
            _cartRepositoryMock
                .Setup(repo => repo.GetCartAsync(_userId, CancellationToken.None))
                .ReturnsAsync(default(Cart));

            // Act
            await _cartJobService.ExecuteJobAsync(_userId);

            // Assert
            _cartRepositoryMock.Verify(repo => repo.DeleteCartAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Never);
            _cartJobClientMock.Verify(client => client.Delete(It.IsAny<string>()), Times.Never);
            _jobRepositoryMock.Verify(repo => repo.DeleteJobIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task DeleteJobAsync_ShouldDeleteJob_WhenJobIdExists()
        {
            // Arrange
            var jobId = "job-123";
            _jobRepositoryMock
                .Setup(repo => repo.GetJobIdAsync(_userId, CancellationToken.None))
                .ReturnsAsync(jobId);

            // Act
            await _cartJobService.DeleteJobAsync(_userId, CancellationToken.None);

            // Assert
            _cartJobClientMock.Verify(client => client.Delete(jobId), Times.Once);
            _jobRepositoryMock.Verify(repo => repo.DeleteJobIdAsync(_userId, CancellationToken.None), Times.Once);
        }

        [Fact]
        public async Task DeleteJobAsync_ShouldDoNothing_WhenJobIdDoesNotExist()
        {
            // Arrange
            _jobRepositoryMock
                .Setup(repo => repo.GetJobIdAsync(_userId, CancellationToken.None))
                .ReturnsAsync((string?)null);

            // Act
            await _cartJobService.DeleteJobAsync(_userId, CancellationToken.None);

            // Assert
            _cartJobClientMock.Verify(client => client.Delete(It.IsAny<string>()), Times.Never);
            _jobRepositoryMock.Verify(repo => repo.DeleteJobIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Never);
        }
    }
}
