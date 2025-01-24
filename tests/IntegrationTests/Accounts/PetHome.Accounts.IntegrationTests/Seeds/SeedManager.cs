using Microsoft.Extensions.DependencyInjection;
using PetHome.Accounts.Infrastructure.Database;
using PetHome.Accounts.IntegrationTests.IntegrationFactories;

namespace PetHome.Accounts.IntegrationTests.Seeds;
public partial class SeedManager
{
    protected readonly AuthorizationDbContext _dbContext; 
    public SeedManager(IntegrationTestFactory factory)
    {
        var scope = factory.Services.CreateScope();
        _dbContext = scope.ServiceProvider.GetRequiredService<AuthorizationDbContext>();
    }
}
