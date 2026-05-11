using FluentValidation;
using FoodOrderingSystem.Application.Abstractions.Messaging;
using FoodOrderingSystem.Application.Common.Behaviour;
using FoodOrderingSystem.Application.Common.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FoodOrderingSystem.Application;

public static class DependencyInjection
{
    private const string JwtSectionName = "Jwt";
    private const string DbSectionName = "Db";
    private const string CachingSectionName = "Redis";

    public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
    {
        return services
            .AddOptions(configuration)
            .AddHandlers()
            .AddValidators()
            .AddTransactionDecorators() // order matters: transaction decorators should be first because the last registered decorator is the outermost one
            .AddFluentValidationDecorators();
    }

    private static IServiceCollection AddOptions(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddOptions<JwtOptions>()
            .BindConfiguration(JwtSectionName);

        services.AddOptions<DbOptions>()
            .BindConfiguration(DbSectionName);;

        return services;
    }

    private static IServiceCollection AddHandlers(this IServiceCollection services)
    {
        services.Scan(scan => scan
            .FromAssemblyOf<AssemblyReference>()
            .AddClasses(c => c.AssignableTo(typeof(ICommandHandler<,>)))
                .AsImplementedInterfaces()
                .WithScopedLifetime()
            .AddClasses(c => c.AssignableTo(typeof(ICommandHandler<>)))
                .AsImplementedInterfaces()
                .WithScopedLifetime()
            .AddClasses(c => c.AssignableTo(typeof(IQueryHandler<,>)))
                .AsImplementedInterfaces()
                .WithScopedLifetime());

        return services;
    }

    private static IServiceCollection AddValidators(this IServiceCollection services)
    {
        services.AddValidatorsFromAssemblyContaining<AssemblyReference>(includeInternalTypes: true);
        return services;
    }

    private static IServiceCollection AddFluentValidationDecorators(this IServiceCollection services)
    {
        services.Decorate(typeof(ICommandHandler<,>), typeof(ValidationDecorator<,>));
        services.Decorate(typeof(ICommandHandler<>), typeof(ValidationDecorator<>));

        return services;
    }

    private static IServiceCollection AddTransactionDecorators(this IServiceCollection services)
    {
        services.Decorate(typeof(ICommandHandler<,>), typeof(TransactionDecorator<,>));
        services.Decorate(typeof(ICommandHandler<>), typeof(TransactionDecorator<>));

        return services;
    }
}
