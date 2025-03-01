using CSharpFunctionalExtensions;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using PetHome.Accounts.Contracts.Messaging.UserManagment;
using PetHome.Core.Constants;
using PetHome.Core.Extentions.ErrorExtentions;
using PetHome.Core.Interfaces.FeatureManagment;
using PetHome.Core.Response.ErrorManagment;
using PetHome.Core.Response.Validation.Validator;
using PetHome.Core.ValueObjects.User;
using PetHome.Framework.Database;
using PetHome.VolunteerRequests.Application.Database.Interfaces;
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
        [FromKeyedServices(Constants.VOLUNTEER_REQUEST_UNIT_OF_WORK_KEY)] IUnitOfWork unitOfWork)
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

        var createVolunteerAccountMessage = new CreatedVolunteerAccountEvent(
            command.Email,
            command.UserName,
            command.StartVolunteeringDate,
            command.Requisites,
            command.Certificates);
        await _publisher.Publish(createVolunteerAccountMessage);

        await _unitOfWork.SaveChanges(ct);
        transaction.Commit();

        return Result.Success<ErrorList>();
    }
}
