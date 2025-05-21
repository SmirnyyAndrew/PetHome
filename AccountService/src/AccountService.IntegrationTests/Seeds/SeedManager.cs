using AccountService.IntegrationTests.IntegrationFactories;
using Microsoft.Extensions.DependencyInjection;
using AccountService.Infrastructure.Database;

namespace AccountService.IntegrationTests.Seeds;
public partial class SeedManager
{
    protected readonly AuthorizationDbContext _dbContext; 
    public SeedManager(IntegrationTestFactory factory)
    {
        var scope = factory.Services.CreateScope();
        _dbContext = scope.ServiceProvider.GetRequiredService<AuthorizationDbContext>();
    }
}
