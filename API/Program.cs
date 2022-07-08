using API.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services
    .AddApplicationServices(builder.Configuration)
    .AddIdentityServices(builder.Configuration);

var app = builder.Build();

await app.MigrateDatabase();

app.UseApplicationMiddleware();

app.Run();
