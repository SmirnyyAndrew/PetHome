using Microsoft.Extensions.DependencyInjection;
using PetManagementService.Domain.PetManagment.PetEntity;
using PetManagementService.Domain.PetManagment.VolunteerEntity;
using PetManagementService.Domain.SpeciesManagment.SpeciesEntity;
using PetManagementService.Infrastructure.Database.Write.DBContext;
using PetManagementService.IntegrationTests.Seeds.SpeciesEntity;
using Xunit;

namespace PetManagementService.IntegrationTests.IntegrationFactories;
public class PetManagementFactory
    : IClassFixture<IntegrationTestFactory>, IAsyncLifetime
{
    private readonly IntegrationTestFactory _factory;
    protected readonly IServiceScope _scope;
    private readonly SeedManager _seedManager;
    protected readonly PetManagementWriteDBContext _writeDbContext;

    public PetManagementFactory(IntegrationTestFactory factory)
    {
        _factory = factory;
        _scope = factory.Services.CreateScope();
        _seedManager = new SeedManager(factory);
        _writeDbContext =
            _scope.ServiceProvider.GetRequiredService<PetManagementWriteDBContext>();
    }


    public async Task SeedVolunteersWithAggregates(
        int volunteerCountToSeed = 5,
        int petCountToSeed = 5,
        int shelterCountToSeed = 2,
        int speciesCountToSeed = 4,
        int breedCountForOneSpeciesToSeed = 4)
    {
        await _seedManager.SeedVolunteersWithAggregates(
            volunteerCountToSeed,
            petCountToSeed,
            shelterCountToSeed,
            speciesCountToSeed,
            breedCountForOneSpeciesToSeed);
    }

    public async Task SeedVolunteers(int volunteerCountToSeed)
    {
        await _seedManager.SeedVolunteers(volunteerCountToSeed);
    }

    public async Task SeedPets(
        Species species,
        IEnumerable<PetShelterId> petShelterIds,
        List<Volunteer> volunteers,
        int petCountToSeed)
    {
        await _seedManager.SeedPets(species, petShelterIds, volunteers, petCountToSeed);
    }

    public async Task SeedShelters(int shelterCountToSeed)
    {
        await _seedManager.SeedShelters(shelterCountToSeed);
    }
    public async Task SeedSpecies(int speciesCountToSeed)
    {
        await _seedManager.SeedSpecies(speciesCountToSeed);
    }

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
