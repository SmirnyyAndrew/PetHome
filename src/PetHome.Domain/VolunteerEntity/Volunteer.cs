using CSharpFunctionalExtensions;
using PetHome.Domain.GeneralValueObjects;
using PetHome.Domain.PetEntity;

namespace PetHome.Domain.VolunteerEntity;
public class Volunteer
{
    //Для EF core
    public Volunteer() { }

    private Volunteer(FullName fullName, string email, string description, DateOnly startVolunteeringDate, 
        PhoneNumber phoneNumber,/* List<SocialNetwork> socialNetworkList,*/ Requisites requisites) { }

    public VolunteerId Id { get; private set; }
    public FullName FullName { get; private set; }
    public string? Email { get; private set; }
    public string Description { get; private set; }
    public DateOnly StartVolunteeringDate { get; private set; }
    public IReadOnlyList<Pet> PetList { get; private set; }
    public int HomedPetsCount => GetPetCountByStatusAndVolunteer(PetStatusEnum.isHomed);
    public int FreePetsCount => GetPetCountByStatusAndVolunteer(PetStatusEnum.isFree);
    public int TreatmentPetsCount => GetPetCountByStatusAndVolunteer(PetStatusEnum.isTreatment);
    public PhoneNumber PhoneNumber { get; private set; }
   // public IReadOnlyList<SocialNetwork> SocialNetworkList { get; private set; }
    public Requisites Requisites { get; private set; }
    // TODO: Реализовать метод после конфигурации БД
    private int GetPetCountByStatusAndVolunteer(PetStatusEnum status) => PetList.Where(pet => pet.Status == status && pet.VolunteerId == Id).Count();

    public Result<Volunteer> Create(FullName fullName, string email, string description, DateOnly startVolunteeringDate,
        PhoneNumber phoneNumber, /*List<SocialNetwork> socialNetworkList, */Requisites requisites)
    {
        if (string.IsNullOrWhiteSpace(description))
            return Result.Failure<Volunteer>("Введите описание");

        if (startVolunteeringDate == null)
            return Result.Failure<Volunteer>("Выберите дату начала волонтёрства");


        return new Volunteer(fullName, email, description, startVolunteeringDate, phoneNumber,/* socialNetworkList,*/ requisites) { };
    }
}
