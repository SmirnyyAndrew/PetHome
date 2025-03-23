using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace PetHome.Accounts.Contracts.HttpCommunication;
public static class AccountHttpClientExtentions
{
    public static IServiceCollection AddAccountHttpClient(
        this IServiceCollection services, 
        IConfiguration configuration, 
        string accountsServiceUrl = "http://localhost:5258/contract")
    {  
        services.AddHttpClient<AccountHttpClient>((sp, config) =>
        { 
            config.BaseAddress = new Uri(accountsServiceUrl);
        });

        services.AddSingleton<AccountHttpClient>();

        return services;
    }
}
