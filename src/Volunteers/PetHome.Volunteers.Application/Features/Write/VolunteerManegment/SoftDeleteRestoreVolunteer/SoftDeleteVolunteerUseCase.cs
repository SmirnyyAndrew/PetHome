using CSharpFunctionalExtensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging;
using PetHome.Core.Constants;
using PetHome.Core.Extentions.ErrorExtentions;
using PetHome.Core.Interfaces.FeatureManagment;
using PetHome.Core.Response.ErrorManagment;
using PetHome.Core.Response.Validation.Validator;
using PetHome.Framework.Database;
using PetHome.Volunteers.Application.Database;
using PetHome.Volunteers.Domain.PetManagment.VolunteerEntity;


namespace PetHome.Volunteers.Application.Features.Write.VolunteerManegment.SoftDeleteRestoreVolunteer;
public class SoftDeleteVolunteerUseCase
    : ICommandHandler<Guid, SoftDeleteRestoreVolunteerCommand>
{
    private readonly IVolunteerRepository _volunteerRepository;
    private readonly ILogger<SoftDeleteVolunteerUseCase> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public SoftDeleteVolunteerUseCase(
        IVolunteerRepository volunteerRepository,
        ILogger<SoftDeleteVolunteerUseCase> logger,
      [FromKeyedServices(Constants.VOLUNTEER_UNIT_OF_WORK_KEY)] IUnitOfWork unitOfWork)
    {
        _volunteerRepository = volunteerRepository;
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid, ErrorList>> Execute(
        SoftDeleteRestoreVolunteerCommand command,
        CancellationToken ct)
    {
        var transaction = await _unitOfWork.BeginTransaction(ct);

        Volunteer volunteer = _volunteerRepository.GetById(command.VolunteerId, ct).Result.Value;
        volunteer.SoftDelete();
        await _volunteerRepository.Update(volunteer, ct);

        await _unitOfWork.SaveChages(ct);
        transaction.Commit();

        _logger.LogInformation("Волонтёр с id = {0} и его сущности soft deleted", command.VolunteerId);
        return command.VolunteerId;
    }
}
