using Microsoft.Extensions.Logging;
using PetHome.Application.Database.Read;
using PetHome.Application.Database;
using PetHome.Application.Features.Write.PetManegment.ChangePetInfo;
using PetHome.Application.Interfaces.RepositoryInterfaces;
using CSharpFunctionalExtensions;
using PetHome.Application.Validator;
using PetHome.Domain.PetManagment.PetEntity;
using PetHome.Domain.PetManagment.VolunteerEntity;
using PetHome.Domain.Shared.Error;

namespace PetHome.Application.Features.Write.PetManegment.HardDelete;
public class HardDeleteUseCase
{
    private readonly IVolunteerRepository _volunteerRepository;
    private readonly IReadDBContext _readDBContext;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<ChangePetInfoUseCase> _logger;

    public HardDeleteUseCase(
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
    public async Task<UnitResult<ErrorList>> Execute(
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

        Volunteer volunteer = _volunteerRepository
            .GetById(command.VolunteerId, ct).Result.Value;
        Pet? pet = volunteer.Pets
            .FirstOrDefault(p => p.Id == command.PetId);
        if (pet == null)
        {
            _logger.LogError("Питомец с id = {0} не найдена", command.PetId);
            return (ErrorList)Errors.NotFound($"Питомец с id = {command.PetId}");
        }

        volunteer.Pets.Remove(pet);
        var transaction = await _unitOfWork.BeginTransaction(ct); 
        try
        {
            await _volunteerRepository.Update(volunteer, ct);
            await _unitOfWork.SaveChages(ct);
            transaction.Commit();

            string message = $"Питомец = {command.PetId} успешно hard deleted!";
            _logger.LogInformation(message);
            return Result.Success<ErrorList>();
        }
        catch (Exception)
        {
            transaction.Rollback();
            string message = $"Не удалось hard delete питомца = {command.PetId}";
            _logger.LogError(message);
            return (ErrorList)Errors.Failure(message);
        }
    }
}
