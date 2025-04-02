using CSharpFunctionalExtensions;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using PetHome.Core.Application.Interfaces.FeatureManagement;
using PetHome.Core.Infrastructure.Database;
using PetHome.Core.Web.Extentions.ErrorExtentions;
using PetHome.SharedKernel.Constants;
using PetHome.SharedKernel.Responses.ErrorManagement;
using PetHome.SharedKernel.ValueObjects.User;
using PetHome.SharedKernel.ValueObjects.VolunteerRequest;
using PetHome.VolunteerRequests.Application.Database.Interfaces;
using PetHome.VolunteerRequests.Contracts.Messaging;
using PetHome.VolunteerRequests.Domain;

namespace PetHome.VolunteerRequests.Application.Features.Write.SetVolunteerRequestRejected;
public class SetVolunteerRequestRejectedUseCase
    : ICommandHandler<SetVolunteerRequestRejectedCommand>
{ 
    private readonly IVolunteerRequestRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPublishEndpoint _publisher;

    public SetVolunteerRequestRejectedUseCase(
        IVolunteerRequestRepository repository,
        IPublishEndpoint publisher, 
        [FromKeyedServices(Constants.Database.VOLUNTEER_REQUEST_UNIT_OF_WORK_KEY)] IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _publisher = publisher;
    }


    public async Task<UnitResult<ErrorList>> Execute(
        SetVolunteerRequestRejectedCommand command, CancellationToken ct)
    {
        var volunteerRequest = await _repository.GetById(command.VolunteerRequestId, ct);
        if (volunteerRequest is null)
            return Errors.NotFound(nameof(VolunteerRequest)).ToErrorList();

        UserId adminId = UserId.Create(command.AdminId).Value;
        RequestComment rejectedComment = RequestComment.Create(command.RejectedComment).Value;
        volunteerRequest.SetRejected(adminId, rejectedComment);

        var transaction = await _unitOfWork.BeginTransaction(ct);
        _repository.Update(volunteerRequest);
        await _unitOfWork.SaveChanges(ct);
       
        SetVolunteerRequestRejectedEvent setVolunteerRequestRejectedEvent = new SetVolunteerRequestRejectedEvent(
          volunteerRequest.Id,
            volunteerRequest?.AdminId,
            volunteerRequest?.UserId,
            volunteerRequest?.DiscussionId,
            command.RejectedComment);
        await _publisher.Publish(setVolunteerRequestRejectedEvent, ct);

        transaction.Commit();

        return Result.Success<ErrorList>();
    }
}
