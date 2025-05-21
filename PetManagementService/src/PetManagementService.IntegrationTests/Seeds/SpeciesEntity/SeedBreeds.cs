using Microsoft.EntityFrameworkCore;
using PetManagementService.Domain.SpeciesManagment.BreedEntity;
using PetManagementService.Domain.SpeciesManagment.SpeciesEntity;
using Xunit;
namespace PetManagementService.IntegrationTests.Seeds.SpeciesEntity;
public partial class SeedManager
{
    public async Task<IReadOnlyList<Breed>> SeedBreeds(int breedCountForOneSpeciesToSeed)
    {
        var speciesDto = await _writeDbContext.Species.ToListAsync(CancellationToken.None);
        if (speciesDto.Count() == 0)
            Assert.False(true, $"Добавьте виды питомцев ({nameof(SeedSpecies)})");

        List<Species> specieses = new List<Species>(speciesDto.Count());
        List<Breed> breeds = new List<Breed>(breedCountForOneSpeciesToSeed * speciesDto.Count());

        for (int speciesIndex = 0; speciesIndex < speciesDto.Count(); speciesIndex++)
        {
            Species species = _writeDbContext.Species
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
