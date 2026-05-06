using FoodOrderingSystem.Application.Common.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FoodOrderingSystem.Application;

public static class DependencyInjection
{
    private const string JwtSectionName = "Jwt";
    public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
    {
        return services
            .AddOptions();
    }

    private static IServiceCollection AddOptions(this IServiceCollection services)
    {
        services
           .AddOptions<JwtOptions>()
               .BindConfiguration(JwtSectionName);

        return services;
    }
}
