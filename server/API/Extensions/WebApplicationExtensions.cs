using API.Middleware;
using API.SignalR;
using Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace API.Extensions;

public static class WebApplicationExtensions
{
    internal static async Task MigrateDatabase(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();

        var services = scope.ServiceProvider;

        try
        {
            var context = services.GetRequiredService<DataContext>();
            var userManager = services.GetRequiredService<UserManager<AppUser>>();
            await context.Database.MigrateAsync();
            await Seed.SeedData(context, userManager);
        }
        catch (Exception e)
        {
            var logger = services.GetRequiredService<ILogger<Program>>();
            logger.LogError(e, "An error occured during migration");
        }
    }

    internal static WebApplication UseApplicationMiddleware(this WebApplication app)
    {
        app.UseMiddleware<ExceptionMiddleware>();

        app.UseXContentTypeOptions();
        app.UseReferrerPolicy(opt => opt.NoReferrer());
        app.UseXXssProtection(opt => opt.EnabledWithBlockMode());
        app.UseXfo(opt => opt.Deny());
        app.UseCsp/*ReportOnly*/(opt => opt
            .BlockAllMixedContent()
            .StyleSources(s => s.Self().CustomSources("https://fonts.googleapis.com", "https://cdn.jsdelivr.net"))
            .FontSources(s => s.Self().CustomSources("https://fonts.gstatic.com", "https://cdn.jsdelivr.net", "data:"))
            .FormActions(s => s.Self())
            .FrameAncestors(s => s.Self())
            .ImageSources(s => s.Self().CustomSources("https://res.cloudinary.com", "blob:"))
            .ScriptSources(s => s.Self())
        );

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        else
        {
            app.Use(async (context, next) =>
            {
                context.Response.Headers.Add("Strict-Transport-Security", "max-age=31536000");
                await next.Invoke();
            });
        }

        if (Environment.GetEnvironmentVariable("PORT") is { } port) // Concession to Heroku port magic (https://stackoverflow.com/questions/59434242/asp-net-core-gives-system-net-sockets-socketexception-error-on-heroku)
        {
            app.Urls.Add($"http://*:{port}");
        }


        app.UseRouting();

        app.UseDefaultFiles();
        app.UseStaticFiles();

        app.UseHttpsRedirection();

        app.UseCors("CorsPolicy");

        app.UseAuthentication(); // Who are _you_?
        app.UseAuthorization(); // Do _you_ have access?

        app.MapControllers();
        app.MapHub<ChatHub>("/chat");
        app.MapFallbackToController("Index", "Fallback"); // This is coupled to FallbackController.cs and its Index() method

        return app;
    }
}