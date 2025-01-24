using Microsoft.EntityFrameworkCore;
using PetHome.SharedKernel.Middlewares;
using PetHome.Volunteers.Infrastructure.Database.Write.DBContext;

namespace PetHome.API.Extentions;

public static class ApplicationExtention
{
    public static async Task ApplyAutoMigrations(this WebApplication application)
    {
        await using var scope = application.Services.CreateAsyncScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<VolunteerWriteDbContext>();
        await dbContext.Database.MigrateAsync();
    }

    public static void UseExceptionHandler(this WebApplication application)
    {
        application.UseMiddleware<ExceptionMiddleware>();
    }
}
