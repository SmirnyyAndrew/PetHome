using CSharpFunctionalExtensions;
using PetHome.Core.Interfaces;
using PetHome.Core.ValueObjects;
using PetHome.Domain.Shared;
using PetHome.Domain.Shared.Error;
using PetHome.Volunteers.Domain.PetManagment.VolunteerEntity;

namespace PetHome.Volunteers.Domain.PetManagment.PetEntity;
public class Pet : SoftDeletableEntity
{
    public static List<Pet> Pets { get; set; } = new List<Pet>();

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
        ValueObjectList<Requisites> requisites)
    {
        Id = PetId.Create().Value;
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
        Requisites = requisites;
        VolunteerId = volunteerId;
        ProfileCreateDate = Date.Create(DateTime.UtcNow).Value;
        Medias = new List<Media>();
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
    public ValueObjectList<Requisites> Requisites { get; private set; }
    public Date ProfileCreateDate { get; private set; }
    public VolunteerId VolunteerId { get; private set; }
    public SerialNumber SerialNumber { get; private set; }
    public ValueObjectList<Media> Medias { get; private set; }

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
        ValueObjectList<Requisites> requisites)
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
            requisites);

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
    public UnitResult<Error> UploadMedia(IEnumerable<Media> mediasToUpload)
    {
        List<Media> newMediaFiles = new List<Media>();
        newMediaFiles.AddRange(mediasToUpload);

        IReadOnlyList<Media> oldMedias = Medias.Values.ToList();
        oldMedias.ToList().ForEach(x => newMediaFiles.Add(Media.Create(x.BucketName, x.FileName).Value));

        //Medias = MediaDetails.Create(newMediaFiles).Value;
        Medias = newMediaFiles;

        return Result.Success<Error>();
    }

    //Удалить медиа
    public UnitResult<Error> RemoveMedia(IEnumerable<Media> mediasToDelete)
    {
        List<Media> oldMediaFiles = Medias.Values
            .Select(m => Media.Create(m.BucketName, m.FileName).Value).ToList();

        List<Media> newMediaFiles = oldMediaFiles.Except(mediasToDelete).ToList();

        //Medias = MediaDetails.Create(newMediaFiles).Value;
        Medias = newMediaFiles;

        return Result.Success<Error>();
    }


    //Изменение информации
    public void ChangeInfo(
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
        ValueObjectList<Requisites> requisites)
    {
        Name = name;
        SpeciesId = speciesId;
        Description = description;
        BreedId = breedId;
        Color = color;
        ShelterId = shelterId;
        Weight = weight;
        IsCastrated = isCastrated;
        BirthDate = birthDate;
        IsVaccinated = isVaccinated;
        Status = status;
        VolunteerId = volunteerId;
        Requisites = requisites;
    }

    //Изменение статуса питомца
    public void ChangeStatus(PetStatusEnum newStatus)
    {
        Status = newStatus;
    }

    //Установить главную фотографию
    public void SetMainPhoto(Media media)
    {
        List<Media> medias = new List<Media>() { media };
        medias.AddRange(Medias
            .Select(m => Media.Create(m.BucketName, m.FileName).Value)
            .Except([media])
            .ToList());
        Medias = new ValueObjectList<Media>(medias);
    }
}
