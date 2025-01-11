using Microsoft.Extensions.DependencyInjection;
using PetHome.IntegrationTests.Seeds;
using PetHome.Species.Domain.SpeciesManagment.SpeciesEntity;
using PetHome.Volunteers.Application.Database;
using PetHome.Volunteers.Domain.PetManagment.PetEntity;
using PetHome.Volunteers.Domain.PetManagment.VolunteerEntity;
using PetHome.Volunteers.Infrastructure.Database.Write.DBContext;
using _Species = PetHome.Species.Domain.SpeciesManagment.SpeciesEntity.Species;
using Xunit;

namespace PetHome.IntegrationTests.IntegrationFactories;
public class VolunteerFactory
    : IClassFixture<IntegrationTestFactory>, IAsyncLifetime
{
    private readonly IntegrationTestFactory _factory;
    protected readonly IServiceScope _scope;
    private readonly SeedManager _seedManager;
    protected readonly IVolunteerReadDbContext _readDbContext;
    protected readonly VolunteerWriteDbContext _writeDbContext;

    public VolunteerFactory(IntegrationTestFactory factory)
    {
        _factory = factory;
        _scope = factory.Services.CreateScope();
        _writeDbContext = _scope.ServiceProvider.GetRequiredService<VolunteerWriteDbContext>();
        _readDbContext = _scope.ServiceProvider.GetRequiredService<IVolunteerReadDbContext>();
        _seedManager = new SeedManager(factory);
    }


    public async Task SeedVolunteersWithAggregates(
        int volunteerCountToSeed = 2,
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
        _Species species,
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
