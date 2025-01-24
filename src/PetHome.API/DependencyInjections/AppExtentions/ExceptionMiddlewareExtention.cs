using PetHome.SharedKernel.Middlewares;

namespace PetHome.API.DependencyInjections.AppExtentions;

public static class ExceptionMiddlewareExtention
{
    public static void UseExceptionHandler(this WebApplication application)
    {
        application.UseMiddleware<ExceptionMiddleware>();
    }
}
