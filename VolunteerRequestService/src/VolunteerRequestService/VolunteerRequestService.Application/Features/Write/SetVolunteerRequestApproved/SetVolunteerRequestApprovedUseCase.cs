using CSharpFunctionalExtensions;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PetHome.Core.Application.Interfaces.FeatureManagement;
using PetHome.Core.Infrastructure.Database;
using PetHome.Core.Tests.IntegrationTests.DependencyInjections;
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
    private readonly IHostEnvironment _env;

    public SetVolunteerRequestApprovedUseCase(
        IVolunteerRequestRepository repository,
        IPublishEndpoint publisher,
        IHostEnvironment env,
        [FromKeyedServices(Constants.Database.VOLUNTEER_REQUEST_UNIT_OF_WORK_KEY)] IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _publisher = publisher;
        _env = env;
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

        await _unitOfWork.SaveChanges(ct);

        if (!_env.IsTestEnvironment())
        {
            SetVolunteerRequestApprovedEvent setVolunteerRequestApprovedEvent = new SetVolunteerRequestApprovedEvent(
                volunteerRequest.UserId,
                adminId.Value,
                command.UserName,
                volunteerRequest.Id,
                volunteerRequest.VolunteerInfo?.Value,
                volunteerRequest.CreatedAt.Value);
            await _publisher.Publish(setVolunteerRequestApprovedEvent, ct);
        }

        transaction.Commit();

        return Result.Success<ErrorList>();
    }
}
