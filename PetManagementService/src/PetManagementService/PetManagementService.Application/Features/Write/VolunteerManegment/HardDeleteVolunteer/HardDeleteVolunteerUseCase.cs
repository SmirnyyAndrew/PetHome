using CSharpFunctionalExtensions;
using MassTransit;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PetHome.Core.Application.Interfaces.FeatureManagement;
using PetHome.Core.Infrastructure.Database;
using PetHome.Core.Tests.IntegrationTests.DependencyInjections;
using PetHome.Core.Web.Extentions.ErrorExtentions;
using PetHome.SharedKernel.Responses.ErrorManagement;
using PetManagementService.Application.Database;
using PetManagementService.Contracts.Messaging.Volunteer;

namespace PetManagementService.Application.Features.Write.VolunteerManegment.HardDeleteVolunteer;
public class HardDeleteVolunteerUseCase
    : ICommandHandler<bool, HardDeleteVolunteerCommand>
{
    private readonly IVolunteerRepository _volunteerRepository;
    private readonly ILogger<HardDeleteVolunteerUseCase> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IHostEnvironment _env;
    private readonly IPublishEndpoint _publisher;

    public HardDeleteVolunteerUseCase(
        IVolunteerRepository volunteerRepository,
        IPublishEndpoint publisher,
        ILogger<HardDeleteVolunteerUseCase> logger,
        IUnitOfWork unitOfWork,
        IHostEnvironment env)
    {
        _volunteerRepository = volunteerRepository;
        _logger = logger;
        _publisher = publisher;
        _unitOfWork = unitOfWork;
        _env = env;
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

        if (!_env.IsTestEnvironment())
        {
            HardDeletedVolunteerEvent hardDeletedVolunteerEvent = new HardDeletedVolunteerEvent(command.VolunteerId);
            await _publisher.Publish(hardDeletedVolunteerEvent, ct);
        }

        transaction.Commit();

        _logger.LogInformation("Волонтёр с id = {0} навсегда удалён", command.VolunteerId.Value);
        return result.Value;
    }
}
