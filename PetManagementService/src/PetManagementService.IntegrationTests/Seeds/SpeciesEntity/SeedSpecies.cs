using PetManagementService.Domain.SpeciesManagment.SpeciesEntity;

namespace PetManagementService.IntegrationTests.Seeds.SpeciesEntity;
public partial class SeedManager
{
    public async Task<IReadOnlyList<Species>> SeedSpecies(int speciesCountToSeed = 3)
    {
        List<Species> specieses = new List<Species>(speciesCountToSeed);
        for (int i = 0; i < speciesCountToSeed; i++)
        {
            Species species = Species.Create($"Вид животного {i}").Value;
            specieses.Add(species);
        }

        await _writeDbContext.AddRangeAsync(specieses, CancellationToken.None);
        await _writeDbContext.SaveChangesAsync();
        return specieses;
    }
}
