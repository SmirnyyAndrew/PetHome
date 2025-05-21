using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using PetHome.VolunteerRequests.Infrastructure.Database.Write;
using VolunteerRequestService.IntegrationTests.Seeds;
using Xunit;

namespace VolunteerRequestService.IntegrationTests.IntegrationFactories;
public class VolunteerRequestFactory
    : IClassFixture<IntegrationTestFactory>, IAsyncLifetime
{
    private readonly IntegrationTestFactory _factory;
    protected readonly IServiceScope _scope;
    private readonly SeedManager _seedManager;
    protected readonly VolunteerRequestDbContext _writeDbContext;
    protected readonly IPublishEndpoint publisher;

    public VolunteerRequestFactory(IntegrationTestFactory factory)
    {
        _factory = factory;
        _scope = factory.Services.CreateScope();
        _writeDbContext = _scope.ServiceProvider.GetRequiredService<VolunteerRequestDbContext>();
        publisher = _scope.ServiceProvider.GetRequiredService<IPublishEndpoint>();
        _seedManager = new SeedManager(factory);
    }


    public async Task SeedVolunteerRequests(int volunteerRequestsCountToSeed)
    {
        await _seedManager.SeedVolunteerRequests(volunteerRequestsCountToSeed);
    }


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
