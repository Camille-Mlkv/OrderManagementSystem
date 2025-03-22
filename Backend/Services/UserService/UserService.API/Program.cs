using Microsoft.EntityFrameworkCore;
using UserService.API.Middleware;
using UserService.BusinessLogic;
using UserService.BusinessLogic.Options;
using UserService.DataAccess.Data;
using UserService.DataAccess.DI;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbProvider(builder.Configuration);

builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection("JwtOptions"));
builder.Services.Configure<EmailOptions>(builder.Configuration.GetSection("EmailOptions"));

builder.Services.AddIdentity();
builder.Services.AddPersistence();
builder.Services.AddApplicationServices();
builder.Services.AddValidation();

builder.Services.AddAppAuthentication(builder.Configuration);
builder.Services.AddAppAuthorization();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//using (var scope = app.Services.CreateScope())
//{
//    await DatabaseInitializer.InitializeAsync(scope.ServiceProvider);
//}

app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
