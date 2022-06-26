using Microsoft.EntityFrameworkCore;
using Persistence;

namespace API.Extensions;

public static class WebApplicationExtensions
{
    internal static async Task MigrateDatabase(this WebApplication application)
    {
        using var scope = application.Services.CreateScope();

        var services = scope.ServiceProvider;

        try
        {
            var context = services.GetRequiredService<DataContext>();
            await context.Database.MigrateAsync();
            await Seed.SeedData(context);
        }
        catch (Exception e)
        {
            var logger = services.GetRequiredService<ILogger<Program>>();
            logger.LogError(e, "An error occured during migration");
        }
    }
}