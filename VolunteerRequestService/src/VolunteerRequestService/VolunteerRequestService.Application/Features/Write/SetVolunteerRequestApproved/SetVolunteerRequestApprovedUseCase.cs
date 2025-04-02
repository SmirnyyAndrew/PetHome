using CSharpFunctionalExtensions;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using PetHome.Accounts.Contracts.Messaging.UserManagment;
using PetHome.Core.Application.Interfaces.FeatureManagement; 
using PetHome.Core.Infrastructure.Database;
using PetHome.Core.Web.Extentions.ErrorExtentions;
using PetHome.SharedKernel.Constants;
using PetHome.SharedKernel.Responses.ErrorManagement;
using PetHome.SharedKernel.ValueObjects.User;
using PetHome.VolunteerRequests.Application.Database.Interfaces;
using PetHome.VolunteerRequests.Contracts.Messaging;
using PetHome.VolunteerRequests.Domain;

namespace PetHome.VolunteerRequests.Application.Features.Write.SetVolunteerRequestApproved;
public class SetVolunteerRequestApprovedUseCase
    : ICommandHandler<SetVolunteerRequestApprovedCommand>
{
    private readonly IVolunteerRequestRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPublishEndpoint _publisher; 

    public SetVolunteerRequestApprovedUseCase(
        IVolunteerRequestRepository repository,
        IPublishEndpoint publisher, 
        [FromKeyedServices(Constants.Database.VOLUNTEER_REQUEST_UNIT_OF_WORK_KEY)] IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _publisher = publisher; 
    }


    public async Task<UnitResult<ErrorList>> Execute(SetVolunteerRequestApprovedCommand command, CancellationToken ct)
    {
        var volunteerRequest = await _repository.GetById(command.VolunteerRequestId, ct);
        if (volunteerRequest is null)
            return Errors.NotFound(nameof(VolunteerRequest)).ToErrorList();

        UserId adminId = UserId.Create(command.AdminId).Value;
        volunteerRequest.SetApproved(adminId);

        var transaction = await _unitOfWork.BeginTransaction(ct);
        _repository.Update(volunteerRequest);

        //TODO: применить event из нового shared
        //var createVolunteerAccountMessage = new CreatedVolunteerAccountEvent(
        //    volunteerRequest.UserId,
        //    command.Email,
        //    command.UserName,
        //    command.StartVolunteeringDate,
        //    command.Requisites,
        //    command.Certificates);
        //await _publisher.Publish(createVolunteerAccountMessage);

        await _unitOfWork.SaveChanges(ct);

        SetVolunteerRequestApprovedEvent setVolunteerRequestApprovedEvent = new SetVolunteerRequestApprovedEvent(
            volunteerRequest.UserId,
            adminId.Value,
            command.UserName,
            volunteerRequest.Id,
            volunteerRequest.VolunteerInfo?.Value,
            volunteerRequest.CreatedAt.Value);
        await _publisher.Publish(setVolunteerRequestApprovedEvent, ct);

        transaction.Commit();

        return Result.Success<ErrorList>();
    }
}
