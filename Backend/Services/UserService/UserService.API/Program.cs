using UserService.API.Middleware;
using UserService.BusinessLogic;
using UserService.BusinessLogic.Jwt;
using UserService.DataAccess.DI;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbProvider(builder.Configuration);
builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection("ApiSettings:JwtOptions"));

builder.Services.AddIdentity();
builder.Services.AddPersistence();
builder.Services.AddApplicationServices();
builder.Services.AddValidation();

builder.Services.AddAppAuthentication(builder.Configuration);
builder.Services.AddAppAuthorization();

await builder.Services.SeedRolesData();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
