using AccountService.Infrastructure.DI;
using AccountService.Infrastructure.DI.Auth;
using AccountService.WEB.DI.ApplicationDI;
using AccountService.WEB.DI.InfrastructureDI;
using FilesService.Communication;

namespace AccountService.WEB.DI;

public static class InfrastructureDependencyInjections
{
    public static IServiceCollection AddInfrastructureDependencyInjection(
        this IServiceCollection services, IConfiguration configuration)
    {
        services.ApplyAuthenticationAuthorizeConfiguration(configuration);
        services.AddAccountsInfrastructure(configuration);  
        services.AddDatabases(configuration);
        services.AddMessageBus(configuration);
        services.AddAmazonHttpCommunication(configuration);

        return services;
    }
}
