using FoodOrderingSystem.Api.Exceptions;

namespace FoodOrderingSystem.Api;

public static class DependencyInjection
{
    public static IServiceCollection AddApi(this IServiceCollection services)
    {
        return services
            .AddSwagger()
            .AddExceptionHandler();
    }

    private static IServiceCollection AddSwagger(this IServiceCollection services)
    {
        return services
            .AddSwaggerGen();
    }

    private static IServiceCollection AddExceptionHandler(this IServiceCollection services)
    {
        return services
            .AddExceptionHandler<GlobalExceptionHandler>()
            .AddProblemDetails();
    }
}
