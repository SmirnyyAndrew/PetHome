using NotificationService.Application.gRPC;
using PetHome.Accounts.Contracts;

namespace NotificationService.DependencyInjections.InfrastructureDependencyInjections;

public static  class GRPCDependencyInjection
{
    public static IServiceCollection AddGRPCClients(this IServiceCollection services)
    {
        services.AddGrpcClient<AccountGRPC.AccountGRPCClient>(g =>
        {
            g.Address = new Uri("https://localhost:7105");
        });

        services.AddSingleton<AccountGRPCService>();

        return services;
    }
}
