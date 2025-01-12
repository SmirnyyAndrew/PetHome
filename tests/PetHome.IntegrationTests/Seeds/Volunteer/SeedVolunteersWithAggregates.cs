
namespace PetHome.IntegrationTests.Seeds;
public partial class SeedManager
{
    public async Task SeedVolunteersWithAggregates(
        int volunteerCountToSeed = 2,
        int petCountToSeed = 5,
        int shelterCountToSeed = 2,
        int speciesCountToSeed = 4,
        int breedCountForOneSpeciesToSeed = 4)
    {
        var volunteers = await SeedVolunteers(volunteerCountToSeed);
        var shelters = await SeedShelters(shelterCountToSeed);
        var species = await SeedSpecies(speciesCountToSeed);
        var breeds = await SeedBreeds(breedCountForOneSpeciesToSeed);

        int speciesRandomIndex = new Random().Next(0, species.Count());
        var pets = await SeedPets(
                species[speciesRandomIndex],
                shelters.Select(s => s.Id),
                volunteers,
                petCountToSeed);
    }
}
