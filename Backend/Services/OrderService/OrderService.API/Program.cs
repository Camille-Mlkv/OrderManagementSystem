using OrderService.Application;
using OrderService.Infrastructure.Configuration;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;
using OrderService.API.Middleware;
using OrderService.Infrastructure.DI;
using Serilog;
using OrderService.Infrastructure.Hubs;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

MongoDbConfiguration.RegisterCustomSerializers();
builder.Services.ConfigureDbServices(builder.Configuration);

builder.Services.ConfigureAuth(builder.Configuration);

builder.Services.ConfigureApplicationServices();

builder.Services.ConfigureCartGrpcService(builder.Configuration);
builder.Services.ConfigureMealGrpcService(builder.Configuration);
builder.Services.ConfigureUserGrpcService(builder.Configuration);

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

LoggingConfiguration.ConfigureLogging(builder.Configuration);
builder.Host.UseSerilog();

builder.Services.ConfigureRabbitMQ(builder.Configuration);

builder.Services.ConfigureSignalR();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(builder =>
{
    builder.WithOrigins("http://localhost:4200")
       .AllowAnyHeader()
       .AllowAnyMethod()
       .AllowCredentials();
});

app.UseAuthentication();
app.UseAuthorization();

app.UseWebSockets();
app.MapControllers();

app.MapHub<OrderHub>("/hubs/order");

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.Run();
