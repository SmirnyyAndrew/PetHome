using CSharpFunctionalExtensions;
using PetHome.Domain.GeneralValueObjects;
using PetHome.Domain.VolunteerEntity;

namespace PetHome.Domain.PetEntity;
public class Pet
{
    //Для EF core
    private Pet(string name, Species species, string description, Breed breed, Color color, PetShelter address, double weight,
        PhoneNumber phoneNumber, bool isCastrated, VO_Date birthDate, bool isVaccinated, PetStatusEnum status, Requisites requisites, VO_Date profileCreateDate) { }

    public PetId Id { get; private set; }
    public string Name { get; private set; }
    public Species Species;
    public string Description { get; private set; }
    public Breed Breed { get; private set; }
    public Color Color { get; private set; }
    public PetShelter Address { get; private set; }
    public double Weight { get; private set; }
    public bool IsCastrated { get; private set; }
    public VO_Date? BirthDate { get; private set; }
    public bool IsVaccinated { get; private set; }
    public PetStatusEnum Status;
    public Requisites? Requisites { get; private set; }
    public VO_Date ProfileCreateDate { get; private set; }
    public Volunteer Volunteer { get; private set; }

    public static Result<Pet> Create(string name, Species species, string description, Breed breed, Color color, PetShelter address, double weight,
        PhoneNumber phoneNumber, bool isCastrated, VO_Date birthDate, bool isVaccinated, PetStatusEnum status, Requisites requisites, VO_Date profileCreateDate)
    {
        if (string.IsNullOrWhiteSpace(name))
            return Result.Failure<Pet>("Введите имя");

        if (string.IsNullOrWhiteSpace(description))
            return Result.Failure<Pet>("Введите описание");

        if (color == null)
            return Result.Failure<Pet>("Выберите цвет животного");

        if (weight > 500 || weight <= 0)
            return Result.Failure<Pet>("Введите корректный вес");


        return new Pet(name, species, description, breed, color, address, weight, phoneNumber, isCastrated, birthDate, isVaccinated, status, requisites, profileCreateDate) { };
    }
}
