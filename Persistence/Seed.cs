﻿using Bogus;
using Domain;

namespace Persistence;

public class Seed
{
    public static async Task SeedData(DataContext context)
        {
            if (context.Activities.Any()) return;

            var pastFaker = new Faker<Reactivity>()
                .RuleFor(o => o.Title, f => f.Lorem.Word())
                .RuleFor(o => o.Date, f => f.Date.Recent())
                .RuleFor(o => o.Description, f => f.Company.CatchPhrase())
                .RuleFor(o => o.Category, f => f.Lorem.Word())
                .RuleFor(o => o.City, f => f.Address.City())
                .RuleFor(o => o.Venue, f => f.Address.BuildingNumber())
                ;
            
            var futureFaker = new Faker<Reactivity>()
                    .RuleFor(o => o.Title, f => f.Lorem.Word())
                    .RuleFor(o => o.Date, f => f.Date.Soon())
                    .RuleFor(o => o.Description, f => f.Company.CatchPhrase())
                    .RuleFor(o => o.Category, f => f.Lorem.Word())
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