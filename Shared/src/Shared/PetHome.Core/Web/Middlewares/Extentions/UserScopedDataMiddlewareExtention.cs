using Microsoft.AspNetCore.Builder;
using PetHome.Core.Web.Middlewares;

namespace PetHome.Core.Web.Middlewares.Extentions;
public static class UserScopedDataMiddlewareExtention
{
    public static WebApplication UseUserScopedDataMiddleware(this  WebApplication app)
    {
        app.UseMiddleware<UserScopedDataMiddleware>();
        return app;
    }
}
