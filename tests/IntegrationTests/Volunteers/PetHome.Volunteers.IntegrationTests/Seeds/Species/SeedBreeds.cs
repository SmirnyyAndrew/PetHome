using Microsoft.EntityFrameworkCore;
using PetHome.Species.Domain.SpeciesManagment.BreedEntity;
using Xunit;
using _Species = PetHome.Species.Domain.SpeciesManagment.SpeciesEntity.Species;
namespace PetHome.Volunteers.IntegrationTests.Seeds.Species;
public partial class SeedManager
{
    public async Task<IReadOnlyList<Breed>> SeedBreeds(int breedCountForOneSpeciesToSeed)
    {
        var speciesDto = await _writeDbContext.Species.ToListAsync(CancellationToken.None);
        if (speciesDto.Count == 0)
            Assert.False(true, $"Добавьте виды питомцев ({nameof(SeedSpecies)})");

        List<_Species> specieses = new List<_Species>(speciesDto.Count);
        List<Breed> breeds = new List<Breed>(breedCountForOneSpeciesToSeed * speciesDto.Count);

        for (int speciesIndex = 0; speciesIndex < speciesDto.Count; speciesIndex++)
        {
            _Species species = _writeDbContext.Species
                .First(s => s.Id == speciesDto[speciesIndex].Id);

            for (int breedNum = 0; breedNum < breedCountForOneSpeciesToSeed; breedNum++)
            {
                Breed breed = Breed.Create($"Вид животного {breedNum}", species.Id).Value;
                breeds.Add(breed);
                _writeDbContext.Update(species);
            }
            species.UpdateBreeds(breeds.Select(b => b.Name.Value));
            specieses.Add(species);
            breeds.Clear();
        }

        _writeDbContext.UpdateRange(specieses);
        await _writeDbContext.SaveChangesAsync();
        return breeds;
    }
}
