using Microsoft.Extensions.DependencyInjection;
using PetHome.IntegrationTests.Seeds;
using PetHome.Species.Application.Database;
using PetHome.Species.Domain.SpeciesManagment.BreedEntity;
using PetHome.Species.Infrastructure.Database.Write.DBContext;
using Xunit;
using _Species = PetHome.Species.Domain.SpeciesManagment.SpeciesEntity.Species;

namespace PetHome.IntegrationTests.IntegrationFactories;
public class SpeciesFactory
    : IClassFixture<IntegrationTestFactory>, IAsyncLifetime
{
    private readonly IntegrationTestFactory _factory;
    protected readonly IServiceScope _scope;
    private readonly SeedManager _seedManager;
    protected readonly ISpeciesReadDbContext _readDbContext;
    protected readonly SpeciesWriteDbContext _writeDbContext;
    public SpeciesFactory(IntegrationTestFactory factory)
    {
        _factory = factory;
        _scope = factory.Services.CreateScope();
        _readDbContext = _scope.ServiceProvider.GetRequiredService<ISpeciesReadDbContext>();
        _writeDbContext = _scope.ServiceProvider.GetRequiredService<SpeciesWriteDbContext>();
        _seedManager = new SeedManager(factory);
    }


     protected async Task<IReadOnlyList<_Species>> SeedSpecies(int speciesCountToSeed = 3)
         => await _seedManager.SeedSpecies(5);
     
     protected async Task<IReadOnlyList<Breed>> SeedBreeds(int breedCountToSedd = 5)
         => await _seedManager.SeedBreeds(breedCountToSedd);
     

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
