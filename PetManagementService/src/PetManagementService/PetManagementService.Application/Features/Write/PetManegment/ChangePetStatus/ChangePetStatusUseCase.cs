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

namespace PetManagementService.Application.Features.Write.PetManegment.ChangePetStatus;
public class ChangePetStatusUseCase
    : ICommandHandler<string, ChangePetStatusCommand>
{
    private readonly IVolunteerRepository _volunteerRepository;
    private readonly IPetManagementReadDbContext _readDBContext;
    private readonly ILogger<ChangePetStatusUseCase> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public ChangePetStatusUseCase(
        IVolunteerRepository volunteerRepository,
        IPetManagementReadDbContext readDBContext,
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
            return Errors.NotFound($"Волонтёр с id = {command.VolunteerId}").ToErrorList();
        }

        var getVolunteerResult = await _volunteerRepository
            .GetById(command.VolunteerId, ct);
        if (getVolunteerResult.IsFailure)
            return getVolunteerResult.Error.ToErrorList();

        Volunteer volunteer = getVolunteerResult.Value;
        Pet? pet = volunteer.Pets
            .FirstOrDefault(p => p.Id == command.PetId);
        if (pet == null)
        {
            _logger.LogError("Питомец с id = {0} не найдена", command.PetId);
            return Errors.NotFound($"Питомец с id = {command.PetId}").ToErrorList();
        }

        pet.ChangeStatus(command.NewPetStatus);

        var transaction = await _unitOfWork.BeginTransaction(ct);

        await _volunteerRepository.Update(volunteer, ct);
        await _unitOfWork.SaveChanges(ct);
        transaction.Commit();

        string message = $"Статус питомца = {command.PetId} изменён";
        _logger.LogInformation(message);
        return message;
    }
}
