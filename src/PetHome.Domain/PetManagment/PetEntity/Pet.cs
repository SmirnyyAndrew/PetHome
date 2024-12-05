using CSharpFunctionalExtensions;
using PetHome.Domain.GeneralValueObjects;
using PetHome.Domain.PetManagment.VolunteerEntity;
using PetHome.Domain.Shared.Error;

namespace PetHome.Domain.PetManagment.PetEntity;
public class Pet
{
    private Pet() { }

    private Pet(
        PetId Id,
        PetName name,
        SpeciesId speciesId,
        string description,
        BreedId breedId,
        Color color,
        PetShelterId shelterId,
        double weight,
        bool isCastrated,
        Date birthDate,
        bool isVaccinated,
        PetStatusEnum status,
        RequisitesDetails requisitesDetails,
        Date profileCreateDate)
    {
        Id = Id;
        Name = name;
        SpeciesId = speciesId;
        Description = description;
        BreedId = breedId;
        Color = color;
        ShelterId = shelterId;
        Weight = weight;
        IsCastrated = isCastrated;
        IsVaccinated = isVaccinated;
        Status = status;
        RequisitesDetails = requisitesDetails;
        ProfileCreateDate = profileCreateDate;
    }


    public PetId Id { get; private set; }
    public PetName Name { get; private set; }
    public SpeciesId SpeciesId;
    public string Description { get; private set; }
    public BreedId? BreedId { get; private set; }
    public Color Color { get; private set; }
    public PetShelterId ShelterId { get; private set; }
    public double Weight { get; private set; }
    public bool IsCastrated { get; private set; }
    public Date? BirthDate { get; private set; }
    public bool IsVaccinated { get; private set; }
    public PetStatusEnum Status;
    public RequisitesDetails? RequisitesDetails { get; private set; }
    public Date ProfileCreateDate { get; private set; }
    public VolunteerId VolunteerId { get; private set; }

    public static Result<Pet, Error> Create(
        PetId id,
        PetName name,
        SpeciesId speciesId,
        string description,
        BreedId breedId,
        Color color,
        PetShelterId address,
        double weight,
        bool isCastrated,
        Date birthDate,
        bool isVaccinated,
        PetStatusEnum status,
        RequisitesDetails requisitesDetails,
        Date profileCreateDate)
    {

        if (string.IsNullOrWhiteSpace(description))
            return Errors.Validation("Описание");

        if (weight > 500 || weight <= 0)
            return Errors.Validation("Вес");

        return new Pet(id, name, speciesId, description, breedId, color, address, weight, isCastrated, birthDate, isVaccinated, status, requisitesDetails, profileCreateDate) { };
    }
}
