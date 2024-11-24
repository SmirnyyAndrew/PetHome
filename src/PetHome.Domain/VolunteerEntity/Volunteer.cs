using CSharpFunctionalExtensions;
using PetHome.Domain.GeneralValueObjects;
using PetHome.Domain.PetEntity;

namespace PetHome.Domain.VolunteerEntity;
public class Volunteer
{
    //Для EF core
    private Volunteer(string fullName, string email, string description, DateOnly startVolunteeringDate, 
        PhoneNumber phoneNumber, List<SocialNetwork> socialNetworkList, Requisites requisites) { }

    public Guid Id { get; private set; }
    public string FullName { get; private set; }
    public string? Email { get; private set; }
    public string Description { get; private set; }
    public DateOnly StartVolunteeringDate { get; private set; }
    public IReadOnlyList<Pet> PetList { get; private set; }
    public int HomedPetsCount => GetPetCountByStatusAndVolunteer(PetStatusEnum.isHomed);
    public int FreePetsCount => GetPetCountByStatusAndVolunteer(PetStatusEnum.isFree);
    public int TreatmentPetsCount => GetPetCountByStatusAndVolunteer(PetStatusEnum.isTreatment);
    public PhoneNumber PhoneNumber { get; private set; }
    public IReadOnlyList<SocialNetwork> SocialNetworkList { get; private set; }
    public Requisites Requisites { get; private set; }
    // TODO: Реализовать метод после конфигурации БД
    private int GetPetCountByStatusAndVolunteer(PetStatusEnum status) => PetList.Where(p => p.Status == status && p.Volunteer.Id == Id).Count();

    public Result<Volunteer> Create(string fullName, string email, string description, DateOnly startVolunteeringDate,
        PhoneNumber phoneNumber, List<SocialNetwork> socialNetworkList, Requisites requisites)
    {
        if (string.IsNullOrWhiteSpace(fullName))
            return Result.Failure<Volunteer>("Введите имя");

        if (string.IsNullOrWhiteSpace(description))
            return Result.Failure<Volunteer>("Введите описание");

        if (startVolunteeringDate == null)
            return Result.Failure<Volunteer>("Выберите дату начала волонтёрства");


        return new Volunteer(fullName, email, description, startVolunteeringDate, phoneNumber, socialNetworkList, requisites) { };
    }
}
