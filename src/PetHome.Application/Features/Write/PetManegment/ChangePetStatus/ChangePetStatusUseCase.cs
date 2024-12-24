using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using PetHome.Application.Database;
using PetHome.Application.Database.Read;
using PetHome.Application.Interfaces.FeatureManagment;
using PetHome.Application.Interfaces.RepositoryInterfaces;
using PetHome.Application.Validator;
using PetHome.Domain.PetManagment.PetEntity;
using PetHome.Domain.PetManagment.VolunteerEntity;
using PetHome.Domain.Shared.Error;

namespace PetHome.Application.Features.Write.PetManegment.ChangePetStatus;
public class ChangePetStatusUseCase
    : ICommandHandler<string, ChangePetStatusCommand>
{
    private readonly IVolunteerRepository _volunteerRepository;
    private readonly IReadDBContext _readDBContext;
    private readonly ILogger<ChangePetStatusUseCase> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public ChangePetStatusUseCase(
        IVolunteerRepository volunteerRepository,
        IReadDBContext readDBContext,
        ILogger<ChangePetStatusUseCase> logger,
        IUnitOfWork unitOfWork)
    {
        _volunteerRepository = volunteerRepository;
        _readDBContext = readDBContext;
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<string, ErrorList>> Execute(
        ChangePetStatusCommand command,
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

        pet.ChangeStatus(command.NewPetStatus);
        var transaction = await _unitOfWork.BeginTransaction(ct);
        try
        {
            await _volunteerRepository.Update(volunteer, ct);
            await _unitOfWork.SaveChages(ct);
            transaction.Commit();

            string message = $"Статус питомца = {command.PetId} изменён";
            _logger.LogInformation(message);
            return message;
        }
        catch (Exception)
        {
            transaction.Rollback();
            string message = $"Не удалось изменить статус питомца = {command.PetId}";
            _logger.LogError(message);
            return (ErrorList)Errors.Failure(message);
        }
    }
}
