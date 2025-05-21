using PetHome.Core.Domain.Models;
using PetHome.SharedKernel.ValueObjects.MainInfo;
using PetHome.SharedKernel.ValueObjects.PetManagment.Extra;
using PetHome.SharedKernel.ValueObjects.User;
using PetManagementService.Domain.PetManagment.VolunteerEntity;
namespace PetManagementService.IntegrationTests.Seeds.SpeciesEntity;

public partial class SeedManager
{
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
            ValueObjectList<PhoneNumber> phoneNumbers = new List<string>()
            { "89888888888", "837347373633", "837347373633", "837347373633" }
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

            UserId userId = UserId.Create().Value;
            volunteer.SetUserId(userId);
            volunteers.Add(volunteer);
        }

        await _writeDbContext.AddRangeAsync(volunteers, CancellationToken.None);
        await _writeDbContext.SaveChangesAsync(CancellationToken.None);
        return volunteers;
    }
}
