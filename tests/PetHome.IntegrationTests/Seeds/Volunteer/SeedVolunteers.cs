using PetHome.Core.Models;
using PetHome.Core.ValueObjects.MainInfo;
using PetHome.Core.ValueObjects.PetManagment.Extra;
using PetHome.Core.ValueObjects.PetManagment.Volunteer;
using PetHome.Volunteers.Domain.PetManagment.VolunteerEntity;

namespace PetHome.IntegrationTests.Seeds.Species;
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
}
