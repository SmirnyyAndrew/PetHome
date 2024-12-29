using FluentValidation; 
using PetHome.Core.Interfaces.FeatureManagment;

namespace PetHome.API.Inject;

public static class Inject
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.Scan(scan => scan.FromAssemblies(typeof(Inject).Assembly)
        .AddClasses(classes => classes
            .AssignableToAny(
                typeof(ICommandHandler<>), typeof(ICommandHandler<,>),
                typeof(IQueryHandler<>), typeof(IQueryHandler<,>)))
        .AsSelfWithInterfaces()
        .WithScopedLifetime());

        services.AddValidatorsFromAssembly(typeof(Inject).Assembly);

        return services;
    }
}
