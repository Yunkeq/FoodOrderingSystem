using FoodOrderingSystem.Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FoodOrderingSystem.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            return services
                .AddPostgres(configuration);
        }

    private static IServiceCollection AddPostgres(this IServiceCollection services, IConfiguration configuration)
        {
            return services
                .AddDbContext<ApplicationDbContext>(options =>
                {
                    options.UseNpgsql(configuration["Db:ConnectionString"] ?? throw new ArgumentException("Db connection string is not specified."));
                });
    }
}
