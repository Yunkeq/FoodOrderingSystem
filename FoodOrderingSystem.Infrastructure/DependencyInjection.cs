using System.Text;
using FoodOrderingSystem.Application.Common.Options;
using FoodOrderingSystem.Application.Common.Security;
using FoodOrderingSystem.Domain.Enums;
using FoodOrderingSystem.Infrastructure.Persistance;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace FoodOrderingSystem.Infrastructure;

public static class DependencyInjection
{
    private const string DbConnectionString = "Db:ConnectionString";

    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration,
        IOptions<JwtOptions> jwtOptions)
    {
        return services
            .AddPostgres(configuration)
            .AddIdentity()
            .AddJwtAuthentication(jwtOptions)
            .AddAuthorization();
    }

    private static IServiceCollection AddPostgres(this IServiceCollection services, IConfiguration configuration)
    {
        return services
            .AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseNpgsql(configuration[DbConnectionString] ?? throw new ArgumentException("Db connection string is not specified."));
            });
    }

    private static IServiceCollection AddIdentity(this IServiceCollection services)
    {
        services.AddIdentityCore<IdentityUser>(options =>
        {
            options.User.RequireUniqueEmail = true;
            options.User.RequireUniqueEmail = true;
            options.Password.RequiredLength = 8;
            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(3);
            options.Lockout.MaxFailedAccessAttempts = 3;
        })
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

        return services;
    }

    private static IServiceCollection AddJwtAuthentication(this IServiceCollection services, IOptions<JwtOptions> jwtOptions)
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
                   ValidIssuer = jwtOptions.Value.Issuer,
                   ValidAudience = jwtOptions.Value.Audience,
                   IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Value.SigningKey)),
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
}
