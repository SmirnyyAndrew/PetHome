using CSharpFunctionalExtensions;
using PetHome.Domain.PetManagment.GeneralValueObjects;
using PetHome.Domain.PetManagment.PetEntity;
using PetHome.Domain.Shared.Error;
using PetHome.Domain.Shared.Interfaces;

namespace PetHome.Domain.PetManagment.VolunteerEntity;
public class Volunteer : ISoftDeletableEntity
{
    private Volunteer() { }

    private Volunteer(
        VolunteerId id,
        FullName fullName,
        Email email,
        Description description,
        Date startVolunteeringDate,
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
    public Description Description { get; private set; }
    public Date StartVolunteeringDate { get; private set; }
    public List<Pet> Pets { get; private set; }
    public int HomedPetsCount => GetPetCountByStatusAndVolunteer(PetStatusEnum.isHomed);
    public int FreePetsCount => GetPetCountByStatusAndVolunteer(PetStatusEnum.isFree);
    public int TreatmentPetsCount => GetPetCountByStatusAndVolunteer(PetStatusEnum.isTreatment);
    public PhoneNumbersDetails? PhoneNumberDetails { get; private set; }
    public RequisitesDetails? RequisitesDetails { get; private set; }
    public SocialNetworkDetails? SocialNetworkDetails { get; private set; }
    private bool _isDeleted = false;


    private int GetPetCountByStatusAndVolunteer(PetStatusEnum status) => Pets.Where(pet => pet.Status == status && pet.VolunteerId == Id).Count();

    public static Result<Volunteer, Error> Create(
        VolunteerId id,
        FullName fullName,
        Email email,
        Description description,
        Date startVolunteeringDate,
        PhoneNumbersDetails? phoneNumbersDetails,
        SocialNetworkDetails? socialNetworkDetails,
        RequisitesDetails? requisitesDetails)
    {
        return new Volunteer(
            id,
            fullName,
            email,
            description,
            startVolunteeringDate,
            phoneNumbersDetails,
            socialNetworkDetails,
            requisitesDetails)
        { };
    }

    public void UpdateMainInfo(
        FullName fullName,
        Description description,
        PhoneNumbersDetails phoneNumbersDetails,
        Email email)
    {
        FullName = fullName;
        Description = description;
        PhoneNumberDetails = phoneNumbersDetails;
        Email = email;
    }

    public void SoftDelete()
    {
        _isDeleted = true;
        Pets.ForEach(pet => pet.SoftDelete());
    }

    public void SoftRestore()
    {
        _isDeleted = false;
        Pets.ForEach(pet => pet.SoftRestore());
    }
}
