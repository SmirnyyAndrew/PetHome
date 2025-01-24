using _Species = PetHome.Species.Domain.SpeciesManagment.SpeciesEntity.Species;

namespace PetHome.Volunteers.IntegrationTests.Seeds.Species;
public partial class SeedManager
{
    public async Task<IReadOnlyList<_Species>> SeedSpecies(int speciesCountToSeed = 3)
    {
        List<_Species> specieses = new List<_Species>(speciesCountToSeed);
        for (int i = 0; i < speciesCountToSeed; i++)
        {
            _Species species = _Species.Create($"Вид животного {i}").Value;
            specieses.Add(species);
        }

        await _writeDbContext.AddRangeAsync(specieses, CancellationToken.None);
        await _writeDbContext.SaveChangesAsync();
        return specieses;
    }
}
