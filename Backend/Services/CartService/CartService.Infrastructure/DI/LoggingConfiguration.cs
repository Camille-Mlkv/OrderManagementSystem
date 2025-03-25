using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Exceptions;

namespace CartService.Infrastructure.DI
{
    public static class LoggingConfiguration
    {
        public static void ConfigureLogging(IConfiguration configuration)
        {
            var logstashUri = configuration["LogstashUri"];

            Log.Logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .Enrich.WithExceptionDetails()
                .WriteTo.Console()
                .WriteTo.Http(logstashUri!, queueLimitBytes: null)
                .CreateLogger();
        }
    }
}
