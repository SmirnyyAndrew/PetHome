using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PetHome.Core.Constants;
using PetHome.Core.ValueObjects.User;
using PetHome.Core.ValueObjects.VolunteerRequest;
using PetHome.Framework.Database;
using PetHome.VolunteerRequests.Application.Database.Interfaces;
using PetHome.VolunteerRequests.Contracts.Messaging;
using PetHome.VolunteerRequests.Domain;

namespace PetHome.VolunteerRequests.Application.Features.Consumers;
public class CreateVolunteerRequestConsumer : IConsumer<CreatedVolunteerRequestEvent>
{
    private readonly IVolunteerRequestRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<CreateVolunteerRequestConsumer> _logger;

    public CreateVolunteerRequestConsumer(
        IVolunteerRequestRepository repository, 
        ILogger<CreateVolunteerRequestConsumer> logger,
        [FromKeyedServices(Constants.VOLUNTEER_REQUEST_UNIT_OF_WORK_KEY)] IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _logger = logger;
    } 

    public async Task Consume(ConsumeContext<CreatedVolunteerRequestEvent> context)
    {
        var command = context.Message;

        UserId userId = UserId.Create(command.UserId).Value;
        VolunteerInfo volunteerInfo = VolunteerInfo.Create(command.VolunteerInfo).Value;
        VolunteerRequest request = VolunteerRequest.Create(userId, volunteerInfo);

        var transaction = await _unitOfWork.BeginTransaction(CancellationToken.None);
        await _repository.Add(request);
        await _unitOfWork.SaveChanges(CancellationToken.None);
        transaction.Commit();

        return;
    }
}
