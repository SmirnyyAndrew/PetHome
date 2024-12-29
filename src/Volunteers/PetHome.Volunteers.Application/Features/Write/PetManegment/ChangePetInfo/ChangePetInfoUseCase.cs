using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using PetHome.Core.Extentions.ErrorExtentions;
using PetHome.Core.Interfaces.FeatureManagment;
using PetHome.Core.Response.ErrorManagment;
using PetHome.Core.Response.Validation.Validator;
using PetHome.Core.ValueObjects;
using PetHome.Framework.Database;
using PetHome.Species.Domain.SpeciesManagment.BreedEntity;
using PetHome.Species.Domain.SpeciesManagment.SpeciesEntity;
using PetHome.Volunteers.Application.Database;
using PetHome.Volunteers.Domain.PetManagment.PetEntity;
using PetHome.Volunteers.Domain.PetManagment.VolunteerEntity;

namespace PetHome.Volunteers.Application.Features.Write.PetManegment.ChangePetInfo;
public class ChangePetInfoUseCase
    : ICommandHandler<string, ChangePetInfoCommand>
{
    private readonly IVolunteerRepository _volunteerRepository;
    private readonly IVolunteerReadDbContext _readDBContext;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<ChangePetInfoUseCase> _logger;

    public ChangePetInfoUseCase(
         IVolunteerRepository volunteerRepository,
         IVolunteerReadDbContext readDBContext,
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
            return Errors.NotFound($"Волонтёр с id = {command.VolunteerId}").ToErrorList();
        }

        var speciesResult = _readDBContext.Species
           .Where(s => s.Id == command.SpeciesId);
        if (speciesResult.Count() == 0)
        {
            _logger.LogError("Вид питомца с id = {0} не найден", command.SpeciesId);
            return Errors.NotFound($"Вид питомца с id = {command.SpeciesId}").ToErrorList();
        }

        var isBreedExist = speciesResult
            .SelectMany(b => b.Breeds)
            .Any(x => x.Id == command.BreedId);
        if (isBreedExist == false)
        {
            _logger.LogError("Порода с id = {0} не найдена", command.SpeciesId);
            return Errors.NotFound($"Порода с id = {command.SpeciesId}").ToErrorList();
        }

        Volunteer volunteer = _volunteerRepository
            .GetById(command.VolunteerId, ct).Result.Value;
        Pet? pet = volunteer.Pets
            .FirstOrDefault(p => p.Id == command.PetId);
        if (pet == null)
        {
            _logger.LogError("Питомец с id = {0} не найдена", command.PetId);
            return Errors.NotFound($"Питомец с id = {command.PetId}").ToErrorList();
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
            return Errors.Failure(message).ToErrorList();
        }
    }
}
