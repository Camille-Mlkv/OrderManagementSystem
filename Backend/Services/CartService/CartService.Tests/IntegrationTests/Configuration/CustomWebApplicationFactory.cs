using Microsoft.AspNetCore.Mvc.Testing;
using Testcontainers.Redis;
using Testcontainers.MsSql;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using StackExchange.Redis;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using CartService.Infrastructure.Data;
using Hangfire;
using Microsoft.AspNetCore.Builder;
using CartService.GrpcServer.Services;
using Microsoft.AspNetCore.Server.Kestrel.Core;

namespace CartService.Tests.IntegrationTests.Configuration
{
    public class CustomWebApplicationFactory: WebApplicationFactory<Program>, IAsyncLifetime
    {
        public ConnectionMultiplexer ConnectionMultiplexer;
        private readonly MsSqlContainer _mssqlContainer;
        private readonly RedisContainer _redisContainer;

        public CustomWebApplicationFactory()
        {
            _mssqlContainer = new MsSqlBuilder()
                .WithImage("mcr.microsoft.com/mssql/server:2022-latest")
                .WithPassword("Admin123")
                .WithPortBinding(1433, true)
                .Build();

            _redisContainer = new RedisBuilder()
                .WithImage("redis:latest")
                .WithPortBinding(6379, true)
                .Build();
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureAppConfiguration((context, config) =>
            {
                config.AddJsonFile("appsettings.test.json")
                   .AddInMemoryCollection(new Dictionary<string, string?>
                   {
                      ["ConnectionStrings:Redis"] = $"{_redisContainer.Hostname}:{_redisContainer.GetMappedPublicPort(6379)}",
                      ["ConnectionStrings:Hangfire"] = $"{_mssqlContainer.GetConnectionString()}",
                   });
            });

            builder.ConfigureKestrel(options =>
            {
                options.ConfigureEndpointDefaults(o => o.Protocols = HttpProtocols.Http2);
            });

            builder.ConfigureServices(services =>
            {
                services.AddGrpc();

                ConfigureRedis(services);

                ConfigureHangfire(services);
            });

            builder.Configure(app =>
            {
                app.UseRouting();

                app.UseEndpoints(endpoints =>
                {
                    endpoints.MapGrpcService<CartServiceImpl>();
                });
            });
        }

        public async Task InitializeAsync()
        {
            await _redisContainer.StartAsync();
            await _mssqlContainer.StartAsync();

            var redisConnectionString = _redisContainer.GetConnectionString();
            ConnectionMultiplexer = await ConnectionMultiplexer.ConnectAsync(redisConnectionString);
        }

        public new async Task DisposeAsync()
        {
            await _mssqlContainer.StopAsync();
            await _mssqlContainer.DisposeAsync();

            if (ConnectionMultiplexer != null)
            {
                await ConnectionMultiplexer.CloseAsync();
                ConnectionMultiplexer.Dispose();
            }

            await _redisContainer.StopAsync();
            await _redisContainer.DisposeAsync();
        }

        private void ConfigureRedis(IServiceCollection services)
        {
            var descriptor = services.FirstOrDefault(d => d.ServiceType == typeof(IConnectionMultiplexer));
            if (descriptor != null)
            {
                services.Remove(descriptor);
            }
            services.AddSingleton<IConnectionMultiplexer>(_ => ConnectionMultiplexer);
        }

        private void ConfigureHangfire(IServiceCollection services)
        {
            var connectionString = _mssqlContainer.GetConnectionString();

            var hangfireServerDescriptor = services.SingleOrDefault(s => s.ServiceType == typeof(JobStorage));
            if (hangfireServerDescriptor is not null)
            {
                services.Remove(hangfireServerDescriptor);
            }
            services.AddHangfire(config => config.UseSqlServerStorage(connectionString));
            services.AddHangfireServer();

            var hangfireDbDescriptor = services.SingleOrDefault(s => s.ServiceType == typeof(DbContextOptions<HangfireDbContext>));
            if (hangfireDbDescriptor is not null)
            {
                services.Remove(hangfireDbDescriptor);
            }
            services.AddDbContext<HangfireDbContext>(options =>
               options.UseSqlServer(connectionString));

            using var scope = services.BuildServiceProvider().CreateScope();
            var hangfireDbContext = scope.ServiceProvider.GetRequiredService<HangfireDbContext>();
            hangfireDbContext.Database.Migrate();
        }
    }
}
