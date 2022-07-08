using Bogus;
using Domain;
using Microsoft.AspNetCore.Identity;

namespace Persistence;

public class Seed
{
    private static readonly string[] Categories = 
    {
        "culture",
        "drinks",
        "film",
        "food",
        "music",
        "travel"
    };
    
    public static async Task SeedData(DataContext context, UserManager<AppUser> userManager)
        {
            if (!userManager.Users.Any())
            {
                var users = new List<AppUser>()
                {
                    new() { DisplayName = "Bob", UserName = "bob", Email = "bob@test.com" },
                    new() { DisplayName = "Tom", UserName = "tom", Email = "tom@test.com" },
                    new() { DisplayName = "Jane", UserName = "jane", Email = "jane@test.com" },
                };

                foreach (var appUser in users)
                {
                    await userManager.CreateAsync(appUser, "Passw0rd");
                }
            }

            if (context.Activities.Any()) return;

            var pastFaker = new Faker<Reactivity>()
                .RuleFor(o => o.Title, f => f.Lorem.Word())
                .RuleFor(o => o.Date, f => f.Date.Recent())
                .RuleFor(o => o.Description, f => f.Company.CatchPhrase())
                .RuleFor(o => o.Category, f => f.PickRandom(Categories))
                .RuleFor(o => o.City, f => f.Address.City())
                .RuleFor(o => o.Venue, f => f.Address.BuildingNumber())
                ;
            
            var futureFaker = new Faker<Reactivity>()
                    .RuleFor(o => o.Title, f => f.Lorem.Word())
                    .RuleFor(o => o.Date, f => f.Date.Soon())
                    .RuleFor(o => o.Description, f => f.Company.CatchPhrase())
                    .RuleFor(o => o.Category, f => f.PickRandom(Categories))
                    .RuleFor(o => o.City, f => f.Address.City())
                    .RuleFor(o => o.Venue, f => f.Address.BuildingNumber())
                ;

            var activities = 
                    pastFaker.GenerateLazy(2)
                .Concat(
                    futureFaker.GenerateLazy(8))
                .ToList();

            await context.Activities.AddRangeAsync(activities);
            await context.SaveChangesAsync();
        }
}