using CartService.Infrastructure.Implementations.Repositories;
using Moq;
using StackExchange.Redis;

namespace CartService.Tests.UnitTests.Repositories
{
    public class CartJobRepositoryTests
    {
        private readonly Mock<IDatabase> _databaseMock;
        private readonly CartJobRepository _jobRepository;
        private readonly Guid _userId = Guid.NewGuid();

        public CartJobRepositoryTests()
        {
            _databaseMock = new Mock<IDatabase>();
            var connectionMultiplexerMock = new Mock<IConnectionMultiplexer>();
            connectionMultiplexerMock
                .Setup(x => x.GetDatabase(It.IsAny<int>(), It.IsAny<object>()))
                .Returns(_databaseMock.Object);

            _jobRepository = new CartJobRepository(connectionMultiplexerMock.Object);
        }

        [Fact]
        public async Task SaveJobIdAsync_ShouldStoreJobIdInRedis()
        {
            // Arrange
            var jobId = "job-123";
            var expectedKey = $"cart_job_{_userId}";

            _databaseMock
               .Setup(db => db.StringSetAsync(
                   expectedKey,
                   jobId,
                   null,
                   It.IsAny<bool>(),
                   When.Always,
                   CommandFlags.None))
               .ReturnsAsync(true);

            // Act
            await _jobRepository.SaveJobIdAsync(_userId, jobId, CancellationToken.None);

            // Assert
            _databaseMock.Verify(db =>
               db.StringSetAsync(
                   expectedKey,
                   jobId,
                   null,
                   It.IsAny<bool>(),
                   When.Always,
                   CommandFlags.None), Times.Once);
        }

        [Fact]
        public async Task DeleteJobIdAsync_ShouldRemoveJobIdFromRedis()
        {
            // Arrange
            var expectedKey = $"cart_job_{_userId}";

            _databaseMock
                .Setup(db => db.KeyDeleteAsync(expectedKey, CommandFlags.None))
                .ReturnsAsync(true);

            // Act
            await _jobRepository.DeleteJobIdAsync(_userId, CancellationToken.None);

            // Assert
            _databaseMock.Verify(db =>
                db.KeyDeleteAsync(expectedKey, CommandFlags.None), Times.Once);
        }

        [Fact]
        public async Task GetJobIdAsync_ShouldReturnJobId_WhenExists()
        {
            // Arrange
            var expectedKey = $"cart_job_{_userId}";
            var expectedJobId = "job-456";

            _databaseMock
                .Setup(db => db.StringGetAsync(expectedKey, CommandFlags.None))
                .ReturnsAsync(expectedJobId);

            // Act
            var result = await _jobRepository.GetJobIdAsync(_userId, CancellationToken.None);

            // Assert
            Assert.Equal(expectedJobId, result);
        }

        [Fact]
        public async Task GetJobIdAsync_ShouldReturnNull_WhenNotExists()
        {
            // Arrange
            var expectedKey = $"cart_job_{_userId}";

            _databaseMock
                .Setup(db => db.StringGetAsync(expectedKey, CommandFlags.None))
                .ReturnsAsync(RedisValue.Null);

            // Act
            var result = await _jobRepository.GetJobIdAsync(_userId, CancellationToken.None);

            // Assert
            Assert.Null(result);
        }
    }
}

