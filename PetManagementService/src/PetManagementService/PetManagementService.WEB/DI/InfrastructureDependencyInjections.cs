using FilesService.Communication;
using PetManagementService.Infrastructure.DI;
using PetManagementService.WEB.DI.InfrastructureDI;

namespace PetManagementService.WEB.DI;

public static class InfrastructureDependencyInjections
{
    public static IServiceCollection AddInfrastructureDependencyInjection(
        this IServiceCollection services, IConfiguration configuration)
    { 
        services.AddAmazonHttpCommunication(configuration);
        services.AddMinioHttpCommunication(configuration);
        services.AddPetManagementInfrastructure(configuration);
        services.AddMessageBus(configuration);

        return services;
    }
}
