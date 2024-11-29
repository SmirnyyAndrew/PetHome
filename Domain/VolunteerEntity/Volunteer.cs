using CSharpFunctionalExtensions;
using PetHome.Domain.GeneralValueObjects;
using PetHome.Domain.PetEntity;

namespace PetHome.Domain.VolunteerEntity;
public class Volunteer
{
    private Volunteer() { }

    private Volunteer(
        FullName fullName,
        Email email,
        string description,
        DateOnly startVolunteeringDate,
        PhoneNumbersDetails phoneNumbersDetails,
        SocialNetworkDetails socialNetworkDetails,
        RequisitesDetails requisitesDetails)
    { }

    public VolunteerId Id { get; private set; }
    public FullName FullName { get; private set; }
    public Email? Email { get; private set; }
    public string Description { get; private set; }
    public DateOnly StartVolunteeringDate { get; private set; }
    public IReadOnlyList<Pet> PetList { get; private set; }
    public int HomedPetsCount => GetPetCountByStatusAndVolunteer(PetStatusEnum.isHomed);
    public int FreePetsCount => GetPetCountByStatusAndVolunteer(PetStatusEnum.isFree);
    public int TreatmentPetsCount => GetPetCountByStatusAndVolunteer(PetStatusEnum.isTreatment);
    public PhoneNumbersDetails PhoneNumberDetails { get; private set; }
    public RequisitesDetails RequisitesDetails { get; private set; }
    public SocialNetworkDetails SocialNetworkDetails { get; private set; }


    private int GetPetCountByStatusAndVolunteer(PetStatusEnum status) => PetList.Where(pet => pet.Status == status && pet.VolunteerId == Id).Count();

    public static Result<Volunteer> Create(
        FullName fullName,
        Email email,
        string description,
        DateOnly startVolunteeringDate,
        PhoneNumbersDetails phoneNumbersDetails,
        SocialNetworkDetails socialNetworkDetails,
        RequisitesDetails requisitesDetails)
    {
        if (string.IsNullOrWhiteSpace(description))
            return Result.Failure<Volunteer>("Введите описание");

        if (startVolunteeringDate == null)
            return Result.Failure<Volunteer>("Выберите дату начала волонтёрства");


        return new Volunteer(fullName, email, description, startVolunteeringDate, phoneNumbersDetails, socialNetworkDetails, requisitesDetails) { };
    }
}
