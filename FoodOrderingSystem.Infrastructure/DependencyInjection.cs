using System.Text;
using FoodOrderingSystem.Application.Abstractions.Caching;
using FoodOrderingSystem.Application.Abstractions.Db;
using FoodOrderingSystem.Application.Abstractions.Identity;
using FoodOrderingSystem.Application.Abstractions.Repositories;
using FoodOrderingSystem.Application.Abstractions.Services;
using FoodOrderingSystem.Application.Common.Options;
using FoodOrderingSystem.Application.Common.Security;
using FoodOrderingSystem.Domain.Entities;
using FoodOrderingSystem.Domain.Enums;
using FoodOrderingSystem.Infrastructure.BackgroundServices;
using FoodOrderingSystem.Infrastructure.Identity;
using FoodOrderingSystem.Infrastructure.Persistance;
using FoodOrderingSystem.Infrastructure.Persistance.Repositories;
using FoodOrderingSystem.Infrastructure.Redis.Cart;
using FoodOrderingSystem.Infrastructure.Redis.Restaurants;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using StackExchange.Redis;

namespace FoodOrderingSystem.Infrastructure;

public static class DependencyInjection
{
    private const string DbConnectionStringSection = "Db:ConnectionString";
    private const string RedisSection = "Redis";
    private const string JwtSection = "Jwt";

    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var jwtOptions = configuration.GetSection(JwtSection).Get<JwtOptions>()
            ?? throw new ArgumentException("Jwt options are not specified.");

        return services
            .AddPostgres(configuration)
            .AddIdentity()
            .AddJwtAuthentication(jwtOptions)
            .AddRepositories()
            .AddDbConnectionFactory()
            .AddTokenProvider()
            .AddUserManager()
            .AddRedis(configuration)
            .AddCacheServices()
            .AddBackgroundServices()
            .AddAuthorization();
    }

    private static IServiceCollection AddPostgres(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseNpgsql(configuration[DbConnectionStringSection] ?? throw new ArgumentException("Db connection string is not specified."));
            });

        services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());

        return services;
    }

    private static IServiceCollection AddIdentity(this IServiceCollection services)
    {
        services.AddIdentityCore<ApplicationUser>(options =>
        {
            options.User.RequireUniqueEmail = true;
            options.User.RequireUniqueEmail = true;
            options.Password.RequiredLength = 8;
            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(3);
            options.Lockout.MaxFailedAccessAttempts = 3;
        })
            .AddRoles<ApplicationRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

        return services;
    }

    private static IServiceCollection AddJwtAuthentication(this IServiceCollection services, JwtOptions jwtOptions)
    {
        services
           .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
           .AddJwtBearer(options =>
           {
               options.TokenValidationParameters = new TokenValidationParameters
               {
                   ValidateIssuer = true,
                   ValidateAudience = true,
                   ValidateLifetime = true,
                   ValidateIssuerSigningKey = true,
                   ValidIssuer = jwtOptions.Issuer,
                   ValidAudience = jwtOptions.Audience,
                   IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.SigningKey)),
                   ClockSkew = TimeSpan.Zero,
               };

               options.Events = new JwtBearerEvents()
               {
                   OnMessageReceived = (context) =>
                   {
                       context.Token = context.Request.Cookies[AuthConstants.AccessTokenCookie];

                       return Task.CompletedTask;
                   },
               };
           });

        return services;
    }

    private static IServiceCollection AddAuthorization(this IServiceCollection services)
    {
        services.AddAuthorization(options =>
        {
            options.AddPolicy(AuthorizationPolicies.AdminPriority, policy =>
            {
                policy.RequireRole(UserRole.Admin.ToString());
            });
            options.AddPolicy(AuthorizationPolicies.CustomerPriority, policy =>
            {
                policy.RequireRole(UserRole.Customer.ToString());
            });
        });

        return services;
    }

    private static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IRestaurantRepository, RestaurantRepository>();
        services.AddScoped<IMenuItemRepository, MenuItemRepository>();

        return services;
    }

    private static IServiceCollection AddDbConnectionFactory(this IServiceCollection services)
    {
        return services.AddScoped<IDbConnectionFactory, DbConnectionFactory>();
    }

    private static IServiceCollection AddTokenProvider(this IServiceCollection services)
    {
        return services.AddScoped<ITokenProvider, TokenProvider>();
    }

    private static IServiceCollection AddUserManager(this IServiceCollection services)
    {
        return services.AddScoped<IUserManagerProvider, UserManagerProvider>();
    }

    private static IServiceCollection AddRedis(this IServiceCollection services, IConfiguration configuration)
    {
        var redisConnectionString = configuration
            .GetValue<string>($"{RedisSection}:ConnectionString")
            ?? throw new ArgumentException("Redis connection string is not specified.");

        var redisPassword = configuration.GetValue<string>($"{RedisSection}:Password");

        return services.AddStackExchangeRedisCache(options =>
        {
            options.ConfigurationOptions = new ConfigurationOptions
            {
                EndPoints =
                {
                    redisConnectionString,
                },
                Password = redisPassword,
            };
        });
    }

    private static IServiceCollection AddBackgroundServices(this IServiceCollection services)
    {
        services.AddHostedService<ExpiredRefreshTokensCleanupBackgroundService>();

        return services;
    }

    private static IServiceCollection AddCacheServices(this IServiceCollection services)
    {
        services.AddScoped<IRestaurantCacheService, RestaurantCacheService>();
        services.AddScoped<ICartService, CartService>();

        return services;
    }
}
