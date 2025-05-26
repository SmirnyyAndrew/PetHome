using FilesService.Communication.HttpClients;
using FilesService.Core.Interfaces;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using PetHome.Core.Application.Interfaces.FeatureManagement;
using PetManagementService.Application.Features.Contracts.SpeciesEntity;
using PetManagementService.Contracts.Contracts.Species;

namespace PetManagementService.Application;

public static class ApplicationDependencyInjection
{
    public static IServiceCollection AddPetManagementServices(this IServiceCollection services)
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

        services.AddMemoryCache();

        services.AddScoped<IMinioFilesHttpClient, MinioFilesHttpClient>();
        return services;
    }
}
