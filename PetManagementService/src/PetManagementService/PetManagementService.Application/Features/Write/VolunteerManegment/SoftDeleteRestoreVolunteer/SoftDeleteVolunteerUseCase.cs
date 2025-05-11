using CSharpFunctionalExtensions;
using MassTransit;
using Microsoft.Extensions.Logging;
using PetHome.Core.Application.Interfaces.FeatureManagement;
using PetHome.Core.Infrastructure.Database;
using PetHome.Core.Web.Extentions.ErrorExtentions;
using PetHome.SharedKernel.Responses.ErrorManagement;
using PetManagementService.Application.Database;
using PetManagementService.Contracts.Messaging.Volunteer;
using PetManagementService.Domain.PetManagment.VolunteerEntity;

namespace PetManagementService.Application.Features.Write.VolunteerManegment.SoftDeleteRestoreVolunteer;
public class SoftDeleteVolunteerUseCase
    : ICommandHandler<Guid, SoftDeleteRestoreVolunteerCommand>
{
    private readonly IVolunteerRepository _volunteerRepository;
    private readonly ILogger<SoftDeleteVolunteerUseCase> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPublishEndpoint _publisher;

    public SoftDeleteVolunteerUseCase(
        IVolunteerRepository volunteerRepository,
        ILogger<SoftDeleteVolunteerUseCase> logger,
        IPublishEndpoint publisher,
       IUnitOfWork unitOfWork)
    {
        _volunteerRepository = volunteerRepository;
        _logger = logger;
        _unitOfWork = unitOfWork;
        _publisher = publisher;
    }

    public async Task<Result<Guid, ErrorList>> Execute(
        SoftDeleteRestoreVolunteerCommand command,
        CancellationToken ct)
    {
        var transaction = await _unitOfWork.BeginTransaction(ct);

        var getVolunteerResult = await _volunteerRepository
            .GetById(command.VolunteerId, ct);
        if (getVolunteerResult.IsFailure)
            return getVolunteerResult.Error.ToErrorList();

        Volunteer volunteer = getVolunteerResult.Value;
        volunteer.SoftDelete();
        await _volunteerRepository.Update(volunteer, ct);

        await _unitOfWork.SaveChanges(ct);

        SoftDeletedVolunteerEvent softDeletedVolunteerEvent = new SoftDeletedVolunteerEvent(volunteer.Id);
        await _publisher.Publish(softDeletedVolunteerEvent, ct);

        transaction.Commit();

        _logger.LogInformation("Волонтёр с id = {0} и его сущности soft deleted", command.VolunteerId);
        return command.VolunteerId;
    }
}
