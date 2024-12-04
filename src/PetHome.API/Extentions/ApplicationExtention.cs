using Microsoft.EntityFrameworkCore;
using PetHome.Infrastructure;

namespace PetHome.API.Extentions;

public static class ApplicationExtention
{
    public static async Task ApplyAutoMigrations(this WebApplication application)
    {
        await using var scope = application.Services.CreateAsyncScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDBContext>();
        await dbContext.Database.MigrateAsync();
    }
}
