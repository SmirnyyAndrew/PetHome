using CSharpFunctionalExtensions;
using FilesService.Core.Dto.File;
using PetHome.Core.Interfaces.Database;
using PetHome.Core.Models;
using PetHome.Core.Response.ErrorManagment;
using PetHome.Core.ValueObjects.MainInfo;
using PetHome.Core.ValueObjects.PetManagment.Breed;
using PetHome.Core.ValueObjects.PetManagment.Extra;
using PetHome.Core.ValueObjects.PetManagment.Pet;
using PetHome.Core.ValueObjects.PetManagment.Species;
using PetHome.Core.ValueObjects.PetManagment.Volunteer;

namespace PetHome.Volunteers.Domain.PetManagment.PetEntity;
public class Pet : SoftDeletableEntity<PetId>
{
    public static List<Pet> Pets { get; set; } = new List<Pet>();

    private Pet(PetId id) : base(id) { }

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
        ValueObjectList<Requisites> requisites,
        MediaFile avatar = null) : base(PetId.Create().Value)
    { 
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
        Photos = new List<MediaFile>();
        Avatar = avatar;
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
    public ValueObjectList<MediaFile> Photos { get; private set; }
    public MediaFile? Avatar { get; private set; }

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
        ValueObjectList<Requisites> requisites,
        MediaFile avatar = null)
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
            requisites,
            avatar);

        pet.InitSerialNumber();
        Pets.Add(pet);
        return pet;
    }

    public override void SoftDelete() => base.SoftDelete();
    public override void SoftRestore() => base.SoftRestore();

    // Присвоить serial number = max + 1
    public UnitResult<Error> InitSerialNumber()
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
            InitSerialNumber();
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
    public UnitResult<Error> ChangeSerialNumberToBeginning()
    {
        return ChangeSerialNumber(1);
    }

    //Добавить медиа
    public UnitResult<Error> UploadMedia(IEnumerable<MediaFile> mediasToUpload)
    {
        List<MediaFile> newMediaFiles = new List<MediaFile>(mediasToUpload);

        IReadOnlyList<MediaFile> oldMedias = Photos.Values.ToList();
        oldMedias.ToList().ForEach(x => newMediaFiles.Add(MediaFile.Create(x.BucketName, x.FileName).Value));

        Photos = newMediaFiles;

        return Result.Success<Error>();
    }

    //Удалить медиа
    public UnitResult<Error> RemoveMedia(IEnumerable<MediaFile> mediasToDelete)
    {
        List<MediaFile> oldMediaFiles = Photos.Values
            .Select(m => MediaFile.Create(m.BucketName, m.FileName).Value).ToList();

        List<MediaFile> newMediaFiles = oldMediaFiles.Except(mediasToDelete).ToList();

        Photos = newMediaFiles;

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
    public void SetMainPhoto(MediaFile media)
    {
        List<MediaFile> medias = new List<MediaFile>() { media };
        medias.AddRange(Photos
            .Select(m => MediaFile.Create(m.BucketName, m.FileName).Value)
            .Except([media])
            .ToList());
        Photos = new ValueObjectList<MediaFile>(medias);
    }

    public void SetAvatar(MediaFile avatar)
    {
        Avatar = avatar;
    }
}
