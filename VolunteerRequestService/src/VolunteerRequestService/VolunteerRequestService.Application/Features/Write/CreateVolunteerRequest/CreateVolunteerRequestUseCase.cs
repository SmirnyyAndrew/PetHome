using CSharpFunctionalExtensions;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using PetHome.Core.Application.Interfaces.FeatureManagement;
using PetHome.Core.Infrastructure.Database;
using PetHome.SharedKernel.Constants;
using PetHome.SharedKernel.Responses.ErrorManagement;
using PetHome.SharedKernel.ValueObjects.User;
using PetHome.SharedKernel.ValueObjects.VolunteerRequest;
using PetHome.VolunteerRequests.Application.Database.Interfaces;
using PetHome.VolunteerRequests.Contracts.Messaging;
using PetHome.VolunteerRequests.Domain;

namespace PetHome.VolunteerRequests.Application.Features.Write.CreateVolunteerRequest;
public class CreateVolunteerRequestUseCase
    : ICommandHandler<CreateVolunteerRequestCommand>
{
    private readonly IVolunteerRequestRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPublishEndpoint _publisher;

    public CreateVolunteerRequestUseCase(
        IVolunteerRequestRepository repository,
        IPublishEndpoint publisher,
        [FromKeyedServices(Constants.Database.VOLUNTEER_REQUEST_UNIT_OF_WORK_KEY)] IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _publisher = publisher;
        _unitOfWork = unitOfWork;
    }

    public async Task<UnitResult<ErrorList>> Execute(
        CreateVolunteerRequestCommand command, CancellationToken ct)
    {
        UserId userId = UserId.Create(command.UserId).Value;
        VolunteerInfo volunteerInfo = VolunteerInfo.Create(command.VolunteerInfo).Value;
        VolunteerRequest request = VolunteerRequest.Create(userId, volunteerInfo);

        var transaction = await _unitOfWork.BeginTransaction(ct);
        await _repository.Add(request);
        await _unitOfWork.SaveChanges(ct);

        CreatedVolunteerRequestEvent createdVolunteerRequestEvent = new CreatedVolunteerRequestEvent(
            request.Id,
            userId,
            volunteerInfo?.Value,
            request.CreatedAt.Value);
        await _publisher.Publish(createdVolunteerRequestEvent, ct);

        transaction.Commit();
        
        return Result.Success<ErrorList>();
    }
}
