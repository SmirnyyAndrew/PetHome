using Microsoft.Extensions.DependencyInjection;
using PetHome.Accounts.Domain.Aggregates;
using PetHome.Accounts.Infrastructure.Database;
using PetHome.Accounts.IntegrationTests.Seeds;
using Xunit;

namespace PetHome.Accounts.IntegrationTests.IntegrationFactories;
public class AccountFactory
    : IClassFixture<IntegrationTestFactory>, IAsyncLifetime
{
    private readonly IntegrationTestFactory _factory;
    protected readonly IServiceScope _scope;
    private readonly SeedManager _seedManager; 
    protected readonly AuthorizationDbContext _dbContext;
    public AccountFactory(IntegrationTestFactory factory)
    {
        _factory = factory;
        _scope = factory.Services.CreateScope(); 
        _dbContext = _scope.ServiceProvider.GetRequiredService<AuthorizationDbContext>();
        _seedManager = new SeedManager(factory);
    }


     protected async Task<IReadOnlyList<User>> SeedUsers(int accountsCountToSeed = 3)
         => await _seedManager.SeedUsers(5);
      
      
    public async Task DisposeAsync()
    {
        _scope.Dispose();
        await _factory.ResetDatabaseAsync();
    }

    public Task InitializeAsync()
    {
        return Task.CompletedTask;
    }
}
