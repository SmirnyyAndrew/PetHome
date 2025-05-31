using Microsoft.Extensions.DependencyInjection;
using NotificationService.Infrastructure.Database;
using NotificationService.IntegrationTests.IntegrationFactories;

namespace NotificationService.IntegrationTests.Seeds;
public partial class SeedManager
{
    protected readonly NotificationDbContext _dbContext;
    public SeedManager(IntegrationTestFactory factory)
    {
        var scope = factory.Services.CreateScope();
        _dbContext = scope.ServiceProvider.GetRequiredService<NotificationDbContext>();
    }
}

