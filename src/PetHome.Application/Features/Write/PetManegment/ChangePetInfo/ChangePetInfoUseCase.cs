using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using PetHome.Application.Database;
using PetHome.Application.Database.Read;
using PetHome.Application.Interfaces.RepositoryInterfaces;
using PetHome.Application.Validator;
using PetHome.Domain.PetManagment.GeneralValueObjects;
using PetHome.Domain.PetManagment.PetEntity;
using PetHome.Domain.PetManagment.VolunteerEntity;
using PetHome.Domain.Shared;
using PetHome.Domain.Shared.Error;

namespace PetHome.Application.Features.Write.PetManegment.ChangePetInfo;
public class ChangePetInfoUseCase
{
    private readonly IVolunteerRepository _volunteerRepository;
    private readonly IReadDBContext _readDBContext;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<ChangePetInfoUseCase> _logger;

    public ChangePetInfoUseCase(
         IVolunteerRepository volunteerRepository,
         IReadDBContext readDBContext,
         IUnitOfWork unitOfWork,
         ILogger<ChangePetInfoUseCase> logger)
    {
        _volunteerRepository = volunteerRepository;
        _readDBContext = readDBContext;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result<string, ErrorList>> Execute(
        ChangePetInfoCommand command,
        CancellationToken ct)
    {
        VolunteerDto? volunteerDto = _readDBContext.Volunteers
                    .FirstOrDefault(v => v.Id == command.VolunteerId);
        if (volunteerDto == null)
        {
            _logger.LogError("Волонтёр с id = {0} не найден", command.VolunteerId);
            return (ErrorList)Errors.NotFound($"Волонтёр с id = {command.VolunteerId}");
        }

        var speciesResult = _readDBContext.Species
           .Where(s => s.Id == command.SpeciesId);
        if (speciesResult.Count() == 0)
        {
            _logger.LogError("Вид питомца с id = {0} не найден", command.SpeciesId);
            return (ErrorList)Errors.NotFound($"Вид питомца с id = {command.SpeciesId}");
        }

        var isBreedExist = speciesResult
            .SelectMany(b => b.Breeds)
            .Any(x => x.Id == command.BreedId);
        if (isBreedExist == false)
        {
            _logger.LogError("Порода с id = {0} не найдена", command.SpeciesId);
            return (ErrorList)Errors.NotFound($"Порода с id = {command.SpeciesId}");
        }

        Volunteer volunteer = _volunteerRepository
            .GetById(command.VolunteerId, ct).Result.Value;
        Pet? pet = volunteer.Pets
            .FirstOrDefault(p => p.Id == command.PetId);
        if (pet == null)
        {
            _logger.LogError("Питомец с id = {0} не найдена", command.PetId);
            return (ErrorList)Errors.NotFound($"Питомец с id = {command.PetId}");
        }

        PetName name = PetName.Create(command.Name).Value;
        SpeciesId speciesId = SpeciesId.Create(command.SpeciesId).Value;
        Description description = Description.Create(command.Description).Value;
        BreedId breedId = BreedId.Create(command.BreedId).Value;
        Color color = Color.Create(command.Color).Value;
        PetShelterId shelterId = PetShelterId.Create(command.ShelterId).Value;
        Date birthDate = Date.Create(command.BirthDate).Value;
        VolunteerId volunteerId = VolunteerId.Create(command.VolunteerId).Value;
        ValueObjectList<Requisites> requisites = command.Requisiteses
            .Select(r => Requisites.Create(r.Name, r.Desc, r.PaymentMethod).Value)
            .ToList();

        pet.ChangeInfo(
            name,
            speciesId,
            description,
            breedId,
            color,
            shelterId,
            command.Weight,
            command.IsCastrated,
            birthDate,
            command.IsVaccinated,
            command.Status,
            volunteerId,
            requisites);

        var transaction = await _unitOfWork.BeginTransaction(ct);
        try
        {
            await _volunteerRepository.Update(volunteer, ct);
            await _unitOfWork.SaveChages(ct);
            transaction.Commit();

            string message = $"Информация питомца = {command.PetId} изменена!";
            _logger.LogInformation(message);
            return message;
        }
        catch (Exception)
        {
            transaction.Rollback();
            string message = $"Не удалось изменить информацию питомца = {command.PetId}";
            _logger.LogError(message);
            return (ErrorList)Errors.Failure(message);
        }
    }
}
