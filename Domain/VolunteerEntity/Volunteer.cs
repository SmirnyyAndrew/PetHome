using CSharpFunctionalExtensions;
using PetHome.Domain.GeneralValueObjects;
using PetHome.Domain.PetEntity;
using PetHome.Domain.Shared.Error;

namespace PetHome.Domain.VolunteerEntity;
public class Volunteer
{
    private Volunteer() { }

    private Volunteer( 
        VolunteerId id,
        FullName fullName,
        Email email,
        string description,
        VO_Date startVolunteeringDate,
        PhoneNumbersDetails phoneNumbersDetails,
        SocialNetworkDetails socialNetworkDetails,
        RequisitesDetails requisitesDetails)
    {
        Id = id;
        FullName = fullName;
        Email = email;
        Description = description;
        StartVolunteeringDate = startVolunteeringDate;
        PhoneNumberDetails = phoneNumbersDetails;
        SocialNetworkDetails = socialNetworkDetails;
        RequisitesDetails = requisitesDetails;

    }

    public VolunteerId Id { get; private set; }
    public FullName FullName { get; private set; }
    public Email? Email { get; private set; }
    public string Description { get; private set; }
    public VO_Date StartVolunteeringDate { get; private set; }
    public IReadOnlyList<Pet> PetList { get; private set; }
    public int HomedPetsCount => GetPetCountByStatusAndVolunteer(PetStatusEnum.isHomed);
    public int FreePetsCount => GetPetCountByStatusAndVolunteer(PetStatusEnum.isFree);
    public int TreatmentPetsCount => GetPetCountByStatusAndVolunteer(PetStatusEnum.isTreatment);
    public PhoneNumbersDetails? PhoneNumberDetails { get; private set; }
    public RequisitesDetails? RequisitesDetails { get; private set; }
    public SocialNetworkDetails? SocialNetworkDetails { get; private set; }


    private int GetPetCountByStatusAndVolunteer(PetStatusEnum status) => PetList.Where(pet => pet.Status == status && pet.VolunteerId == Id).Count();

    public static Result<Volunteer, Error> Create(
        VolunteerId id,
        FullName fullName,
        Email email,
        string description,
        VO_Date startVolunteeringDate,
        PhoneNumbersDetails phoneNumbersDetails,
        SocialNetworkDetails socialNetworkDetails,
        RequisitesDetails requisitesDetails)
    {
        if (string.IsNullOrWhiteSpace(description))
            return Errors.Validation("Описание");

        if (startVolunteeringDate == null)
            return Errors.Validation("Дата начала волонтёрства");


        return new Volunteer(id, fullName, email, description, startVolunteeringDate, phoneNumbersDetails, socialNetworkDetails, requisitesDetails) { };
    }
}
