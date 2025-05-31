using Microsoft.Extensions.DependencyInjection;
using NotificationService.Infrastructure.Database;
using NotificationService.IntegrationTests.Seeds;
using Xunit;

namespace NotificationService.IntegrationTests.IntegrationFactories;
public class NotificationFactory
    : IClassFixture<IntegrationTestFactory>, IAsyncLifetime
{
    private readonly IntegrationTestFactory _factory;
    protected readonly IServiceScope _scope;
    private readonly SeedManager _seedManager;
    protected readonly NotificationDbContext _dbContext; 

    public NotificationFactory(IntegrationTestFactory factory)
    {
        _factory = factory;
        _scope = factory.Services.CreateScope();
        _dbContext = _scope.ServiceProvider.GetRequiredService<NotificationDbContext>(); 
        _seedManager = new SeedManager(factory);
    }


    //protected async Task<IReadOnlyList<Role>> SeedRoles()
    //    => await _seedManager.SeedRoles();
     
     
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
