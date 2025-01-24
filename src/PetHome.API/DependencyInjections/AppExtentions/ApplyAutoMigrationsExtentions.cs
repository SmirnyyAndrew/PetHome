using Microsoft.EntityFrameworkCore;
using PetHome.Volunteers.Infrastructure.Database.Write.DBContext;

namespace PetHome.API.DependencyInjections.AppExtentions;

public static class ApplyAutoMigrationsExtentions
{
    public static async Task ApplyAutoMigrations(this WebApplication application)
    {
        await using var scope = application.Services.CreateAsyncScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<VolunteerWriteDbContext>();
        await dbContext.Database.MigrateAsync();
    }
}
