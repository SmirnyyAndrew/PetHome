using Microsoft.AspNetCore.Builder;

namespace PetHome.Accounts.API.gRPC;
public static class GRPCDependencyInjection
{
    public static WebApplication AddAccountGRPCServices(this WebApplication app)
    {
        app.MapGrpcService<AccountGRPCService>();

        return app;
    }
}
