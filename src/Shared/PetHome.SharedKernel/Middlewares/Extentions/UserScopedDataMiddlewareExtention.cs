using Microsoft.AspNetCore.Builder;

namespace PetHome.SharedKernel.Middlewares.Extentions;
public static class UserScopedDataMiddlewareExtention
{
    public static WebApplication UseUserScopedDataMiddleware(this  WebApplication app)
    {
        app.UseMiddleware<UserScopedDataMiddleware>();
        return app;
    }
}
