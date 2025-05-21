using DiscussionService.Infrastructure.Database.Write;
using Microsoft.Extensions.DependencyInjection;
using PetHome.Discussions.Domain;
using PetHome.Discussions.IntegrationTests.Seeds;
using Xunit;

namespace PetHome.Discussions.IntegrationTests.IntegrationFactories;
public class DiscussionFactory
    : IClassFixture<IntegrationTestFactory>, IAsyncLifetime
{
    private readonly IntegrationTestFactory _factory;
    protected readonly IServiceScope _scope;
    private readonly SeedManager _seedManager;
    protected readonly DiscussionDbContext _dbContext;

    public DiscussionFactory(IntegrationTestFactory factory)
    {
        _factory = factory;
        _scope = factory.Services.CreateScope();
        _dbContext = _scope.ServiceProvider.GetRequiredService<DiscussionDbContext>();
        _seedManager = new SeedManager(factory);
    }


    protected async Task<IReadOnlyList<Discussion>> SeedDiscussions(int discussionsCountToSeed = 3)
        => await _seedManager.SeedDiscussions(discussionsCountToSeed);


    public async Task DisposeAsync()
    {
        await _factory.ResetDatabaseAsync();
        _scope.Dispose();
    }

    public Task InitializeAsync()
    {
        return Task.CompletedTask;
    }
}
