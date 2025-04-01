using Microsoft.AspNetCore.Builder;

namespace AccountService.API.gRPC;
public static class GRPCDependencyInjection
{
    public static WebApplication AddAccountGRPCServices(this WebApplication app)
    {
        app.MapGrpcService<AccountGRPCService>();

        return app;
    }
}
