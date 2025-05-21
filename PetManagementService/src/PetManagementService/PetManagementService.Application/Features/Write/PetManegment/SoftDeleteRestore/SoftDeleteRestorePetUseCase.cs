using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using PetHome.Core.Application.Interfaces.FeatureManagement;
using PetHome.Core.Infrastructure.Database;
using PetHome.Core.Web.Extentions.ErrorExtentions;
using PetHome.SharedKernel.Responses.ErrorManagement;
using PetManagementService.Application.Database;
using PetManagementService.Application.Database.Dto;
using PetManagementService.Domain.PetManagment.PetEntity;
using PetManagementService.Domain.PetManagment.VolunteerEntity;

namespace PetManagementService.Application.Features.Write.PetManegment.SoftDeleteRestore;
public class SoftDeleteRestorePetUseCase
    : ICommandHandler<SoftDeleteRestorePetCommand>
{
    private readonly IVolunteerRepository _volunteerRepository;
    private readonly IPetManagementReadDbContext _readDBContext;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<SoftDeleteRestorePetUseCase> _logger;

    public SoftDeleteRestorePetUseCase(
        IVolunteerRepository volunteerRepository,
        IPetManagementReadDbContext readDBContext,
        IUnitOfWork unitOfWork,
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
         
        var getVolunteerResult = await _volunteerRepository
            .GetById(command.VolunteerId, ct);
        if (getVolunteerResult.IsFailure)
            return getVolunteerResult.Error.ToErrorList();

        Volunteer volunteer = getVolunteerResult.Value;
        Pet? pet = volunteer.Pets
            .ToList()
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

        await _volunteerRepository.Update(volunteer, ct);
        await _unitOfWork.SaveChanges(ct);
        transaction.Commit();

        string message = $"Питомец = {command.PetId} успешно soft deleted!";
        _logger.LogInformation(message);
        return Result.Success<ErrorList>(); 
    }
}
