using CSharpFunctionalExtensions;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PetHome.Core.Constants;
using PetHome.Core.Extentions.ErrorExtentions;
using PetHome.Core.Interfaces.FeatureManagment;
using PetHome.Core.Response.Validation.Validator;
using PetHome.Framework.Database;
using PetHome.Species.Contracts.Messaging;
using PetHome.Volunteers.Application.Database;
using PetHome.Volunteers.Contracts.Messaging;

namespace PetHome.Volunteers.Application.Features.Write.VolunteerManegment.HardDeleteVolunteer;
public class HardDeleteVolunteerUseCase
    : ICommandHandler<bool, HardDeleteVolunteerCommand>
{
    private readonly IVolunteerRepository _volunteerRepository;
    private readonly ILogger<HardDeleteVolunteerUseCase> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPublishEndpoint _publisher;

    public HardDeleteVolunteerUseCase(
        IVolunteerRepository volunteerRepository,
        IPublishEndpoint publisher,
        ILogger<HardDeleteVolunteerUseCase> logger,
       [FromKeyedServices(Constants.VOLUNTEER_UNIT_OF_WORK_KEY)] IUnitOfWork unitOfWork)
    {
        _volunteerRepository = volunteerRepository;
        _logger = logger;
        _publisher = publisher;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<bool, ErrorList>> Execute(
        HardDeleteVolunteerCommand command,
        CancellationToken ct)
    {
        var transaction = await _unitOfWork.BeginTransaction(ct);

        var result = _volunteerRepository.RemoveById(command.VolunteerId, ct).Result;

        if (result.IsFailure)
            return result.Error.ToErrorList();

        await _unitOfWork.SaveChanges(ct);

        HardDeletedVolunteerEvent hardDeletedVolunteerEvent = new HardDeletedVolunteerEvent(command.VolunteerId);
        await _publisher.Publish(hardDeletedVolunteerEvent, ct);
        
        transaction.Commit();

        _logger.LogInformation("Волонтёр с id = {0} навсегда удалён", command.VolunteerId.Value);
        return result.Value;
    }
}
