using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using PetHome.Core.Interfaces.FeatureManagment;
using PetHome.Species.Application.Features.Contracts.Species;
using PetHome.Species.Contracts.Contracts.Species;

namespace PetHome.Species.Application;

public static class ApplicationDependencyInjection
{
    public static IServiceCollection AddSpeciesServices(this IServiceCollection services)
    {
        services.Scan(scan => scan.FromAssemblies(typeof(ApplicationDependencyInjection).Assembly)
        .AddClasses(classes => classes
            .AssignableToAny(
                typeof(ICommandHandler<>), typeof(ICommandHandler<,>),
                typeof(IQueryHandler<>), typeof(IQueryHandler<,>)))
        .AsSelfWithInterfaces()
        .WithScopedLifetime());
         
        services.AddScoped<IGetSpeciesIdContract, GetSpeciesIdUsingContract>();

        services.AddValidatorsFromAssembly(typeof(ApplicationDependencyInjection).Assembly);

        services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssembly(typeof(ApplicationDependencyInjection).Assembly);
        });

        return services;
    }
}
