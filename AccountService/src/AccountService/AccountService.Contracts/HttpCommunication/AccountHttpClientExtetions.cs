using Microsoft.Extensions.DependencyInjection;

namespace AccountService.Contracts.HttpCommunication;
public static class AccountHttpClientExtentions
{
    public static IServiceCollection AddAccountHttpClient(
        this IServiceCollection services,
        string accountsServiceUrl = "http://localhost:5258/contract/")
    {
        services.AddHttpClient<AccountHttpClient>((sp, config) =>
        {
            config.BaseAddress = new Uri(accountsServiceUrl);
        });

        return services;
    }
}
