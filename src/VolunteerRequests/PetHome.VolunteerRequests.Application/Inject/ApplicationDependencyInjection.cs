using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using PetHome.Core.Interfaces.FeatureManagment;
using PetHome.VolunteerRequests.Application.Features.Contracts.CreateVolunteerRequest;
using PetHome.VolunteerRequests.Contracts;

namespace PetHome.VolunteerRequests.Application.Inject;
public static class ApplicationDependencyInjection
{
    public static IServiceCollection AddVolunteerRequestsServices(this IServiceCollection services)
    {
        services.Scan(scan => scan.FromAssemblies(typeof(ApplicationDependencyInjection).Assembly)
        .AddClasses(classes => classes
            .AssignableToAny(
                typeof(ICommandHandler<>), typeof(ICommandHandler<,>),
                typeof(IQueryHandler<>), typeof(IQueryHandler<,>)))
        .AsSelfWithInterfaces()
        .WithScopedLifetime());

        services.AddScoped<ICreateVolunteerRequestContract,CreateVolunteerRequestUsingContract>();
 
        services.AddValidatorsFromAssembly(typeof(ApplicationDependencyInjection).Assembly);

        return services;
    }
}
