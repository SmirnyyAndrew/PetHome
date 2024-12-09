using CSharpFunctionalExtensions;
using PetHome.Domain.PetManagment.GeneralValueObjects;
using PetHome.Domain.PetManagment.VolunteerEntity;
using PetHome.Domain.Shared.Error;
using PetHome.Domain.Shared.Interfaces;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;

namespace PetHome.Domain.PetManagment.PetEntity;
public class Pet : SoftDeletableEntity
{
    public static List<Pet> Pets { get; private set; } = new List<Pet>();

    private Pet() { }

    private Pet(
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
        VolunteerId volunteerId,
        RequisitesDetails requisitesDetails)
    {
        Id = PetId.Create();
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
        VolunteerId = volunteerId;
        ProfileCreateDate = Date.Create(DateTime.UtcNow).Value;
        MediaDetails = MediaDetails.Create().Value;
    }


    public PetId Id { get; private set; }
    public PetName Name { get; private set; }
    public SpeciesId SpeciesId;
    public Description Description { get; private set; }
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
    public SerialNumber SerialNumber { get; private set; }
    public MediaDetails MediaDetails { get; private set; }

    public static Result<Pet, Error> Create(
        PetName name,
        SpeciesId speciesId,
        Description description,
        BreedId breedId,
        Color color,
        PetShelterId ShelterId,
        double weight,
        bool isCastrated,
        Date birthDate,
        bool isVaccinated,
        PetStatusEnum status,
        VolunteerId volunteerId,
        RequisitesDetails requisitesDetails)
    {
        if (weight > 500 || weight <= 0)
            return Errors.Validation("Вес");

        Pet pet = new Pet(
            name,
            speciesId,
            description,
            breedId,
            color,
            ShelterId,
            weight,
            isCastrated,
            birthDate,
            isVaccinated,
            status,
            volunteerId,
            requisitesDetails);

        pet.InitSerialNumer();
        Pets.Add(pet);
        return pet;
    }

    public override void SoftDelete() => base.SoftDelete();
    public override void SoftRestore() => base.SoftRestore();

    // Присвоить serial number = max + 1
    public UnitResult<Error> InitSerialNumer()
    {
        SerialNumber serialNumber = Pets.Count == 0
            ? SerialNumber.Create(1)
            : SerialNumber.Create(Pets.Select(x => x.SerialNumber.Value).Max() + 1);

        SerialNumber = serialNumber;
        return Result.Success<Error>();
    }

    //Изменить serial number
    public UnitResult<Error> ChangeSerialNumber(int number)
    {
        if (Pets.Count == 0)
        {
            InitSerialNumer();
            return Result.Success<Error>();
        }

        int maxSerialNumber = Pets.Max(s => s.SerialNumber.Value);
        if (number > maxSerialNumber || number < 1)
        {
            return Errors.Conflict($"Новый серийный номер {number} не можеть превышать максимальное значение {maxSerialNumber} и быть меньше 1");
        }

        if (number > SerialNumber.Value)
        {
            Pets.Where(p =>
              p.SerialNumber.Value > SerialNumber.Value
              && p.SerialNumber.Value <= number)
          .ToList()
         .ForEach(s => s.SerialNumber = SerialNumber.Create(s.SerialNumber.Value - 1));
        }
        else if (number < SerialNumber.Value)
        {
            Pets.Where(p =>
               p.SerialNumber.Value < SerialNumber.Value
               && p.SerialNumber.Value >= number)
           .ToList()
           .ForEach(s => s.SerialNumber = SerialNumber.Create(s.SerialNumber.Value + 1));
        }

        SerialNumber = SerialNumber.Create(number);

        Pets = Pets.OrderBy(x => x.SerialNumber.Value).ToList();

        return Result.Success<Error>();
    }

    // Присвоить serial number =  1
    public UnitResult<Error> ChangeSerialNumberToBegining()
    {
        return ChangeSerialNumber(1);
    }

    //Добавить медиа
    public UnitResult<Error> UploadMedia(IEnumerable<Media> oldMedias, IEnumerable<Media> uploadMedias)
    {
        List<Media> currentMedias = new List<Media>();
        currentMedias.AddRange(uploadMedias);
        oldMedias.ToList().ForEach(x => currentMedias.Add(Media.Create(x.BucketName,x.FileName).Value));

        MediaDetails = MediaDetails.Create(currentMedias).Value;

        return Result.Success<Error>();
    }

    //Удалить медиа
    public UnitResult<Error> RemoveMedia(IEnumerable<Media> oldMedia, IEnumerable<Media> removeMedia)
    {
        List<Media> currentMedias = new List<Media>();
        currentMedias.AddRange(oldMedia);
        currentMedias = currentMedias.Except(removeMedia).ToList();

        MediaDetails = MediaDetails.Create(currentMedias).Value;

        return Result.Success<Error>();
    }
}
