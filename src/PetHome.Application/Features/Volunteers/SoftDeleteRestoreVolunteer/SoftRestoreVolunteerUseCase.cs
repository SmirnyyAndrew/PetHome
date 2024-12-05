using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using PetHome.Domain.PetManagment.VolunteerEntity;
using PetHome.Domain.Shared.Error;

namespace PetHome.Application.Features.Volunteers.SoftDeleteRestoreVolunteer;
public class SoftRestoreVolunteerUseCase
{
    private readonly IVolunteerRepository _volunteerRepository;
    private readonly ILogger<SoftRestoreVolunteerUseCase> _logger;

    public SoftRestoreVolunteerUseCase(
        IVolunteerRepository volunteerRepository,
        ILogger<SoftRestoreVolunteerUseCase> logger)
    {
        _volunteerRepository = volunteerRepository;
        _logger = logger;
    }

    public async Task<Result<Guid,Error>> Execute(
        Guid id, CancellationToken ct)
    {
        Volunteer volunteer = _volunteerRepository.GetById(id, ct).Result.Value;
        volunteer.SoftRestore();
        await _volunteerRepository.Update(volunteer, ct);

        _logger.LogInformation("Волонтёр с id = {0} и его сущности soft restored", id);

        return id;
    }
}
