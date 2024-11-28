using CSharpFunctionalExtensions;
using PetHome.Domain.GeneralValueObjects;
using PetHome.Domain.VolunteerEntity;

namespace PetHome.Domain.PetEntity;
public class Pet
{
    private Pet() { }

    private Pet(
        PetName name,
        SpeciesId speciesId,
        string description,
        BreedId breedId,
        Color color,
        PetShelterId address,
        double weight, 
        bool isCastrated,
        VO_Date birthDate,
        bool isVaccinated,
        PetStatusEnum status,
        RequisitesDetails requisitesDetails,
        VO_Date profileCreateDate)
    { }

    
    public PetId Id { get; private set; }
    public PetName Name { get; private set; }
    public SpeciesId SpeciesId;
    public string Description { get; private set; }
    public BreedId BreedId { get; private set; }
    public Color Color { get; private set; }
    public PetShelterId ShelterId { get; private set; }
    public double Weight { get; private set; }
    public bool IsCastrated { get; private set; }
    public VO_Date? BirthDate { get; private set; }
    public bool IsVaccinated { get; private set; }
    public PetStatusEnum Status;
    public RequisitesDetails RequisitesDetails { get; private set; }
    public VO_Date ProfileCreateDate { get; private set; }
    public VolunteerId VolunteerId { get; private set; }

    public static Result<Pet> Create(
        PetName name,
        SpeciesId speciesId,
        string description,
        BreedId breedId,
        Color color,
        PetShelterId address,
        double weight, 
        bool isCastrated,
        VO_Date birthDate,
        bool isVaccinated,
        PetStatusEnum status,
        RequisitesDetails requisitesDetails,
        VO_Date profileCreateDate)
    {


        if (string.IsNullOrWhiteSpace(description))
            return Result.Failure<Pet>("Введите описание");

        if (color == null)
            return Result.Failure<Pet>("Выберите цвет животного");

        if (weight > 500 || weight <= 0)
            return Result.Failure<Pet>("Введите корректный вес");


        return new Pet(name, speciesId, description, breedId, color, address, weight, isCastrated, birthDate, isVaccinated, status, requisitesDetails, profileCreateDate) { };
    }
}
