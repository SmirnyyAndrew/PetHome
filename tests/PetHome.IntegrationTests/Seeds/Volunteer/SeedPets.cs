using PetHome.Core.Models;
using PetHome.Core.ValueObjects.MainInfo;
using PetHome.Core.ValueObjects.PetManagment.Breed;
using PetHome.Core.ValueObjects.PetManagment.Extra;
using PetHome.Core.ValueObjects.PetManagment.Pet;
using PetHome.Core.ValueObjects.PetManagment.Species;
using PetHome.Core.ValueObjects.PetManagment.Volunteer;
using PetHome.Volunteers.Domain.PetManagment.PetEntity;
using PetHome.Volunteers.Domain.PetManagment.VolunteerEntity;
using _Species = PetHome.Species.Domain.SpeciesManagment.SpeciesEntity.Species;

namespace PetHome.IntegrationTests.Seeds;
public partial class SeedManager
{
    public async Task<IReadOnlyList<Pet>> SeedPets(
        _Species species,
        IEnumerable<PetShelterId> petShelterIds,
        List<Volunteer> volunteers,
        int petCountToSeed)
    {
       int breedRandomIndex = new Random().Next(0, _writeDbContext.Species
           .Where(s => s.Id == species.Id)
           .Select(b => b.Breeds)
           .Count());
        int petShelterRandomIndex = new Random().Next(0, petShelterIds.Count());
        int volunteerRandomIndex = new Random().Next(0, volunteers.Count());

        List<Pet> pets = new List<Pet>(petCountToSeed);
        for (int i = 0; i < petCountToSeed; i++)
        {

            PetName name = PetName.Create("Кличка " + i).Value;

            SpeciesId speciesId = species.Id;

            BreedId breedId = _writeDbContext.Species
                .Where(s => s.Id == species.Id)
                .Select(b => b.Breeds)
                .ToList()[breedRandomIndex]
                .Select(b => b.Id)
                .First();

            PetShelterId shelterId =
                PetShelterId.Create(petShelterIds.ToList()[petShelterRandomIndex]).Value;

            VolunteerId volunteerId =
                VolunteerId.Create(volunteers
                .Select(v => v.Id)
                .ToList()[volunteerRandomIndex]).Value;

            Description description = Description.Create("Описание №" + i).Value;
            Date birthDate = Date.Create(DateTime.Now).Value;
            Color color = Color.Create("белый").Value;
            ValueObjectList<Requisites> requisites = new List<Requisites>();
            PetStatusEnum status = PetStatusEnum.isFree;
            double weight = 20d;
            bool isCastrated = false;
            bool isVaccinated = false;

            volunteers[volunteerRandomIndex].CreatePet(
                name,
                speciesId,
                description,
                breedId,
                color,
                shelterId,
                weight,
                isCastrated,
                birthDate,
                isVaccinated,
                status,
                requisites);
        }

        _writeDbContext.UpdateRange(volunteers);
        await _writeDbContext.SaveChangesAsync(CancellationToken.None);
        return pets;
    }
}
