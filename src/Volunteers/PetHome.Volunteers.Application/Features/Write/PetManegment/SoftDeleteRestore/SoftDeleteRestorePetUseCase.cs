using CSharpFunctionalExtensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PetHome.Core.Constants;
using PetHome.Core.Extentions.ErrorExtentions;
using PetHome.Core.Interfaces.FeatureManagment;
using PetHome.Core.Response.ErrorManagment;
using PetHome.Core.Response.Validation.Validator;
using PetHome.Framework.Database;
using PetHome.Volunteers.Application.Database;
using PetHome.Volunteers.Domain.PetManagment.PetEntity;
using PetHome.Volunteers.Domain.PetManagment.VolunteerEntity;


namespace PetHome.Volunteers.Application.Features.Write.PetManegment.SoftDeleteRestore;
public class SoftDeleteRestorePetUseCase
    : ICommandHandler<SoftDeleteRestorePetCommand>
{
    private readonly IVolunteerRepository _volunteerRepository;
    private readonly IVolunteerReadDbContext _readDBContext;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<SoftDeleteRestorePetUseCase> _logger;

    public SoftDeleteRestorePetUseCase(
        IVolunteerRepository volunteerRepository,
        IVolunteerReadDbContext readDBContext,
       [FromKeyedServices(Constants.VOLUNTEER_UNIT_OF_WORK_KEY)] IUnitOfWork unitOfWork,
        ILogger<SoftDeleteRestorePetUseCase> logger)
    {
        _volunteerRepository = volunteerRepository;
        _readDBContext = readDBContext;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<UnitResult<ErrorList>> Execute(
        SoftDeleteRestorePetCommand command,
        CancellationToken ct)
    {
        VolunteerDto? volunteerDto = _readDBContext.Volunteers
                    .FirstOrDefault(v => v.Id == command.VolunteerId);
        if (volunteerDto == null)
        {
            _logger.LogError("Волонтёр с id = {0} не найден", command.VolunteerId);
            return Errors.NotFound($"Волонтёр с id = {command.VolunteerId}").ToErrorList();
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

        if (command.ToDelete)
            pet.SoftDelete();
        else
            pet.SoftRestore();

        var transaction = await _unitOfWork.BeginTransaction(ct);
        try
        {
            await _volunteerRepository.Update(volunteer, ct);
            await _unitOfWork.SaveChages(ct);
            transaction.Commit();

            string message = $"Питомец = {command.PetId} успешно soft deleted!";
            _logger.LogInformation(message);
            return Result.Success<ErrorList>();
        }
        catch (Exception)
        {
            transaction.Rollback();
            string message = $"Не удалось soft delete питомца = {command.PetId}";
            _logger.LogError(message);
            return Errors.Failure(message).ToErrorList();
        }
    }
}
