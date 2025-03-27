using Elastic.Ingest.Elasticsearch;
using Elastic.Ingest.Elasticsearch.DataStreams;
using Elastic.Serilog.Sinks;
using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Exceptions;

namespace OrderService.Infrastructure.DI
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
