using AutoFixture;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PetHome.Core.ValueObjects;
using PetHome.Species.Domain.SpeciesManagment.BreedEntity;
using PetHome.Species.Domain.SpeciesManagment.SpeciesEntity;
using PetHome.Volunteers.Application.Database;
using PetHome.Volunteers.Domain.PetManagment.PetEntity;
using PetHome.Volunteers.Domain.PetManagment.VolunteerEntity;
using PetHome.Volunteers.Infrastructure.Database.Write.DBContext;
using _Species = PetHome.Species.Domain.SpeciesManagment.SpeciesEntity.Species;
using Xunit;

namespace PetHome.IntegrationTests.IntegrationFactories;
public class BaseFactory
    : IClassFixture<IntegrationTestFactory>, IAsyncLifetime
{
    protected readonly IntegrationTestFactory _factory;
    protected readonly Fixture _fixture;
    protected readonly IServiceScope _scope;
    protected readonly IVolunteerReadDbContext _readDbContext;
    protected readonly VolunteerWriteDBContext _writeDbContext;

    public BaseFactory(IntegrationTestFactory factory)
    {
        _factory = factory;
        _fixture = new Fixture();
        _scope = factory.Services.CreateScope();
        _readDbContext = _scope.ServiceProvider.GetRequiredService<IVolunteerReadDbContext>();
        _writeDbContext = _scope.ServiceProvider.GetRequiredService<VolunteerWriteDBContext>();
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


    public async Task SeedVolunteersWithAggregates(
        int volunteerCountToSeed = 2,
        int petCountToSeed = 5,
        int shelterCountToSeed = 2,
        int speciesCountToSeed = 4,
        int breedCountForOneSpeciesToSeed = 4)
    {
        int speciesRandomIndex;

        var volunteers = await SeedVolunteers(volunteerCountToSeed);
        var shelters = await SeedShelters(shelterCountToSeed);
        var species = await SeedSpecies(speciesCountToSeed);
        var breeds = await SeedBreeds(breedCountForOneSpeciesToSeed);

        speciesRandomIndex = new Random().Next(0, species.Count());
        var pets = await SeedPets(
                species[speciesRandomIndex],
                shelters.Select(s => s.Id),
                volunteers,
                petCountToSeed);
    }

    public async Task<List<Volunteer>> SeedVolunteers(int volunteerCountToSeed)
    {
        List<Volunteer> volunteers = new List<Volunteer>(volunteerCountToSeed);
        for (int i = 0; i < volunteerCountToSeed; i++)
        {
            VolunteerId volunteerId = VolunteerId.Create().Value;
            FullName fullName = FullName.Create("Имя" + i, "Фамилия" + i).Value;
            Email email = Email.Create($"email{i}@mail.ru").Value;
            Description description = Description.Create($"Описание_{i}").Value;
            Date startVolunteeringDate = Date.Create(DateTime.Now).Value;
            ValueObjectList<PhoneNumber> phoneNumbers = new List<string>() { "89888888888", "837347373633", "837347373633", "837347373633" }
                .Select(p => PhoneNumber.Create(p).Value)
                .ToList();
            ValueObjectList<Requisites> requisites = new List<Requisites>().ToList();
            ValueObjectList<SocialNetwork> socialNetworks = new List<string>() { "vk.com/8912412", "tg.com/24412", "twitter.com/8928712" }
               .Select(p => SocialNetwork.Create(p).Value)
               .ToList();

            var volunteer = Volunteer.Create(
                volunteerId,
                fullName,
                email,
                description,
                startVolunteeringDate,
                phoneNumbers,
                requisites,
                socialNetworks).Value;

            volunteers.Add(volunteer);
        }

        await _writeDbContext.AddRangeAsync(volunteers, CancellationToken.None);
        await _writeDbContext.SaveChangesAsync(CancellationToken.None);
        return volunteers;
    }


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


    public async Task<IReadOnlyList<PetShelter>> SeedShelters(int shelterCountToSeed)
    {
        List<PetShelter> shelters = new List<PetShelter>(shelterCountToSeed);
        for (int i = 0; i < shelterCountToSeed; i++)
        {
            PetShelter shelter = PetShelter.Create("Приют №" + i).Value;
            shelters.Add(shelter);
        }
        return shelters;
    }


    public async Task<IReadOnlyList<_Species>> SeedSpecies(int speciesCountToSeed)
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
