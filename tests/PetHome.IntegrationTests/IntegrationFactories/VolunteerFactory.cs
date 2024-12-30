using Microsoft.Extensions.DependencyInjection;
using PetHome.IntegrationTests.Seeds;
using PetHome.Volunteers.Application.Database;
using PetHome.Volunteers.Domain.PetManagment.PetEntity;
using PetHome.Volunteers.Domain.PetManagment.VolunteerEntity;
using PetHome.Volunteers.Infrastructure.Database.Write.DBContext;

namespace PetHome.IntegrationTests.IntegrationFactories;
public class VolunteerFactory : BaseFactory
{
    private readonly SeedManager _seedManager;
    protected readonly IVolunteerReadDbContext _readDbContext;
    protected readonly VolunteerWriteDbContext _writeDbContext;

    public VolunteerFactory(IntegrationTestFactory factory) : base(factory)
    {
        _readDbContext = _scope.ServiceProvider.GetRequiredService<IVolunteerReadDbContext>();
        _writeDbContext = _scope.ServiceProvider.GetRequiredService<VolunteerWriteDbContext>();
        _seedManager = new SeedManager(factory);
    }


    public IReadOnlyList<Volunteer> SeedVolunteersWithAggregates(
        int volunteerCountToSeed = 2,
        int petCountToSeed = 5,
        int shelterCountToSeed = 2,
        int speciesCountToSeed = 4,
        int breedCountForOneSpeciesToSeed = 4)
        => _seedManager.SeedVolunteersWithAggregates(
            volunteerCountToSeed,
            petCountToSeed,
            shelterCountToSeed,
            speciesCountToSeed,
            breedCountForOneSpeciesToSeed).Result;


    public IReadOnlyList<Volunteer> SeedVolunteers(int volunteerCountToSeed)
    => _seedManager.SeedVolunteers(volunteerCountToSeed).Result;


    public IReadOnlyList<Pet> SeedPets(
        IEnumerable<PetShelterId> petShelterIds,
        List<Volunteer> volunteers,
        int petCountToSeed)
    => _seedManager.SeedPets(petShelterIds, volunteers, petCountToSeed).Result;


    public IReadOnlyList<PetShelter> SeedShelters(int shelterCountToSeed)
        => _seedManager.SeedShelters(shelterCountToSeed).Result;
}
