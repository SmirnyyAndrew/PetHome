using PetHome.Accounts.API.gRPC;

namespace PetHome.API.DependencyInjections.AppExtentions;

public static class GRPCDependencyInjections
{
    public static WebApplication AddGRPCServices(this WebApplication app)
    {
        app.AddAccountGRPCServices();

        return app;
    }
}
