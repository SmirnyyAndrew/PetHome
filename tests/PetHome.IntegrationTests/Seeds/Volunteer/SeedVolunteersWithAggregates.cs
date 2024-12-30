using PetHome.Volunteers.Domain.PetManagment.VolunteerEntity;

namespace PetHome.IntegrationTests.Seeds;
public partial class SeedManager
{
    public async Task<IReadOnlyList<Volunteer>> SeedVolunteersWithAggregates(
        int volunteerCountToSeed = 2,
        int petCountToSeed = 5,
        int shelterCountToSeed = 2,
        int speciesCountToSeed = 4,
        int breedCountForOneSpeciesToSeed = 4)
    {
        int speciesRandomIndex;

        var volunteers = await SeedVolunteers(volunteerCountToSeed);
        var shelters = await SeedShelters(shelterCountToSeed);
        //var species = await SeedSpecies(speciesCountToSeed);
        //var breeds = await SeedBreeds(breedCountForOneSpeciesToSeed);

        //speciesRandomIndex = new Random().Next(0, species.Count());
        var pets = await SeedPets(
                //species[speciesRandomIndex],
                shelters.Select(s => s.Id),
                volunteers,
                petCountToSeed);

        return volunteers;
    }
}
