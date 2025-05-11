using AccountService.Contracts.HttpCommunication;
using PetHome.Core.Application.Interfaces.FeatureManagement;

namespace NotificationService.DependencyInjections.ApplicationDependencyInjections;

public static class ServicesDependencyInjections
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.Scan(scan => scan.FromAssemblies(typeof(ServicesDependencyInjections).Assembly)
        .AddClasses(classes => classes
            .AssignableToAny(
                typeof(ICommandHandler<>), typeof(ICommandHandler<,>),
                typeof(IQueryHandler<>), typeof(IQueryHandler<,>)))
        .AsSelfWithInterfaces()
        .WithScopedLifetime());

        services.AddAccountHttpClient();

        return services;
    }
}
