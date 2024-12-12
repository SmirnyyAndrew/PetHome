using CSharpFunctionalExtensions;
using PetHome.Domain.PetManagment.GeneralValueObjects;
using PetHome.Domain.PetManagment.PetEntity;
using PetHome.Domain.Shared;
using PetHome.Domain.Shared.Error;
using PetHome.Domain.Shared.Interfaces;

namespace PetHome.Domain.PetManagment.VolunteerEntity;
public class Volunteer : SoftDeletableEntity
{
    private Volunteer() { }

    private Volunteer(
        VolunteerId id,
        FullName fullName,
        Email email,
        Description description,
        Date startVolunteeringDate,
        ValueObjectList<PhoneNumber> phoneNumbers,
        ValueObjectList<SocialNetwork> socialNetworks,
        ValueObjectList<Requisites> requisites)
    {
        Id = id;
        FullName = fullName;
        Email = email;
        Description = description;
        StartVolunteeringDate = startVolunteeringDate;
        PhoneNumbers = phoneNumbers;
        SocialNetworks = socialNetworks;
        Requisites = requisites;

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
    public ValueObjectList<PhoneNumber> PhoneNumbers { get; private set; }
    public ValueObjectList<Requisites> Requisites { get; private set; }
    public ValueObjectList<SocialNetwork> SocialNetworks { get; private set; }


    private int GetPetCountByStatusAndVolunteer(PetStatusEnum status) => Pets.Count(pet => pet.Status == status && pet.VolunteerId == Id);

    public static Result<Volunteer, Error> Create(
        VolunteerId id,
        FullName fullName,
        Email email,
        Description description,
        Date startVolunteeringDate,
        ValueObjectList<PhoneNumber> phoneNumbers,
        ValueObjectList<Requisites> requisites,
        ValueObjectList<SocialNetwork> socialNetworks)
    {
        return new Volunteer(
            id,
            fullName,
            email,
            description,
            startVolunteeringDate,
            phoneNumbers,
            socialNetworks,
            requisites)
        { };
    }

    public void UpdateMainInfo(
        FullName fullName,
        Description description,
        ValueObjectList<PhoneNumber> phoneNumbers,
        Email email)
    {
        FullName = fullName;
        Description = description;
        PhoneNumbers = phoneNumbers;
        Email = email;
    }


    public override void SoftDelete()
    {
        base.SoftDelete();
        Pets.ForEach(pet => pet.SoftDelete());
    }

    public override void SoftRestore()
    {
        base.SoftRestore();
        Pets.ForEach(pet => pet.SoftRestore());
    }

    public Result<Pet, Error> CreatePet(
        PetName name,
        SpeciesId speciesId,
        Description description,
        BreedId breedId,
        Color color,
        PetShelterId shelterId,
        double weight,
        bool isCastrated,
        Date birthDate,
        bool isVaccinated,
        PetStatusEnum status,
        ValueObjectList<Requisites> requisites)
    {
        var result = Pet.Create(
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
              Id,
              requisites);
        if (result.IsFailure)
            return result.Error;

        Pet pet = result.Value;
        Pets.Add(pet);

        return pet;
    }
}
