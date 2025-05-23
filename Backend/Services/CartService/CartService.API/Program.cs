using CartService.Application;
using CartService.API.Middleware;
using Hangfire;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;
using CartService.Infrastructure.DI;
using Serilog;

using CartService.Infrastructure.Data;
using CartService.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .AddJsonFile($"appsettings.test.json", optional: true);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.ConfigureCartServices(builder.Configuration);
builder.Services.ConfigureHangfireServices(builder.Configuration);
builder.Services.ConfigureAuth(builder.Configuration);

LoggingConfiguration.ConfigureLogging(builder.Configuration);
builder.Host.UseSerilog();

builder.Services.ConfigureApplicationServices();

builder.Services.ConfigureGrpcConnection(builder.Configuration);

builder.Services.AddHttpContextAccessor();

builder.Services.AddSwaggerGen(option =>
{
    option.AddSecurityDefinition(name: JwtBearerDefaults.AuthenticationScheme, securityScheme: new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Description = "Enter the Bearer authorization string as following:'Bearer Generated-JWT-Token'",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference=new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id=JwtBearerDefaults.AuthenticationScheme
                }
            },new string[] { }
        }
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    using var scope = app.Services.CreateScope();
    await DatabaseInitializer.InitializeHangfireDbAsync(scope.ServiceProvider);
}

var options = new DashboardOptions()
{
    Authorization = [new HangfireDashboardAuthFilter()]
};

app.UseHangfireDashboard("/hangfire", options);

app.MapHangfireDashboard();

app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.MapControllers();

app.Run();

public partial class Program { }