using CSharpFunctionalExtensions;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using PetHome.Core.Constants;
using PetHome.Core.Extentions.ErrorExtentions;
using PetHome.Core.Interfaces.FeatureManagment;
using PetHome.Core.Response.ErrorManagment;
using PetHome.Core.Response.Validation.Validator;
using PetHome.Core.ValueObjects.Discussion;
using PetHome.Core.ValueObjects.User;
using PetHome.Framework.Database;
using PetHome.VolunteerRequests.Application.Database.Interfaces;
using PetHome.VolunteerRequests.Contracts.Messaging;
using PetHome.VolunteerRequests.Domain;

namespace PetHome.VolunteerRequests.Application.Features.Write.SetVolunteerRequestOnReview;
public class SetVolunteerRequestOnReviewUseCase
    : ICommandHandler<SetVolunteerRequestOnReviewCommand>
{
    private readonly IVolunteerRequestRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPublishEndpoint _publisher;

    public SetVolunteerRequestOnReviewUseCase(
        IVolunteerRequestRepository repository,
        IPublishEndpoint publisher,
        [FromKeyedServices(Constants.VOLUNTEER_REQUEST_UNIT_OF_WORK_KEY)] IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _publisher = publisher;
    }

    public async Task<UnitResult<ErrorList>> Execute(
        SetVolunteerRequestOnReviewCommand command, CancellationToken ct)
    {
        var volunteerRequest = await _repository.GetById(command.VolunteerRequestId, ct);
        if (volunteerRequest is null)
            return Errors.NotFound(nameof(VolunteerRequest)).ToErrorList();

        UserId adminId = UserId.Create(command.AdminId).Value;
        DiscussionId discussionId = DiscussionId.Create(command.DiscussionId).Value;
        volunteerRequest.SetOnReview(adminId, discussionId);

        var transaction = await _unitOfWork.BeginTransaction(ct);
        _repository.Update(volunteerRequest);

        var createDiscussionMessage = new SetVolunteerRequestOnReviewEvent(
            volunteerRequest.Id,
            volunteerRequest.AdminId,
            volunteerRequest.UserId,
            volunteerRequest.DiscussionId,
            command.RelationName);
        await _publisher.Publish(createDiscussionMessage);
         
        await _unitOfWork.SaveChanges(ct);
        transaction.Commit();

        return Result.Success<ErrorList>();
    }
}
