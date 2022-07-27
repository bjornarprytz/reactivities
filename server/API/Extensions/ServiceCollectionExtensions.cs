using System.Text;
using API.Services;
using Application.Activities;
using Application.Core;
using Application.Interfaces;
using Domain;
using FluentValidation.AspNetCore;
using Infrastructure.Photos;
using Infrastructure.Security;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Persistence;

namespace API.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
    {
        services.AddControllers(opt =>
            {
                var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();

                opt.Filters.Add(new AuthorizeFilter(policy));
            })
            .AddFluentValidation(opt =>
            {
                opt.RegisterValidatorsFromAssemblyContaining<Create>();
            });
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        services.AddEndpointsApiExplorer();

        services.AddSwaggerGen();

        services.AddDbContext<DataContext>(opt => opt.ConfigureHerokuConnectionString(config));
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true); // Fix for a datetime problem related to .NET6 and PostgreSQL (https://stackoverflow.com/questions/69961449/net6-and-datetime-problem-cannot-write-datetime-with-kind-utc-to-postgresql-ty)

        services.AddCors(options =>
        {
            options.AddPolicy("CorsPolicy", policy =>
            {
                policy.AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials()
                    .WithOrigins("http://localhost:3000"); // TODO: Get this from environment (docker)
            });
        });

        services.AddMediatR(typeof(List.Handler).Assembly);
        services.AddAutoMapper(typeof(MappingProfiles).Assembly);
        services.AddScoped<IUserAccessor, UserAccessor>();
        services.AddScoped<IPhotoAccessor, PhotoAccessor>();
        services.Configure<CloudinarySettings>(config.GetSection("Cloudinary"));
        services.AddSignalR();

        return services;
    }

    public static IServiceCollection AddIdentityServices(this IServiceCollection services, IConfiguration config)
    {
        services.AddIdentityCore<AppUser>(opt =>
        {
            opt.Password.RequireNonAlphanumeric = false;
            opt.User.RequireUniqueEmail = true;
        })
            .AddEntityFrameworkStores<DataContext>()
            .AddSignInManager<SignInManager<AppUser>>();

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["TokenKey"]));

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(opt =>
            {
                opt.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = key,
                    ValidateIssuer = false,
                    ValidateAudience = false
                };

                opt.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        var accessToken = context.Request.Query["access_token"];
                        var path = context.HttpContext.Request.Path;
                        if (!string.IsNullOrEmpty(accessToken) && (path.StartsWithSegments("/chat")))
                        {
                            context.Token = accessToken;
                        }
                        return Task.CompletedTask;
                    }
                };
            });

        services.AddAuthorization(opt =>
        {
            opt.AddPolicy("IsActivityHost", policy =>
            {
                policy.Requirements.Add(new IsHostRequirement());
            });
            opt.AddPolicy("IsProfileOwner", policy =>
            {
                policy.Requirements.Add(new IsProfileOwnerRequirement());
            });
        });
        services.AddTransient<IAuthorizationHandler, IsHostRequirementHandler>();
        services.AddTransient<IAuthorizationHandler, IsProfileOwnerRequirementHandler>();
        services.AddScoped<TokenMintingService>();

        return services;
    }
}