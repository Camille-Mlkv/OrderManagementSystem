using Microsoft.Extensions.Logging;
using Moq;

namespace CartService.Tests.Utilities.Extensions
{
    public static class LoggerMoqExtensions
    {
        public static void VerifyLog<T>(
           this Mock<ILogger<T>> loggerMock,
           LogLevel level,
           string? messageSubstring = null,
           Times? times = null)
        {
            times ??= Times.Once();

            loggerMock.Verify(
                x => x.Log(
                    level,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) =>
                        messageSubstring == null || v.ToString().Contains(messageSubstring)),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                times.Value);
        }
    }
}
