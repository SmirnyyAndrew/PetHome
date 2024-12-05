using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using PetHome.Domain.Shared.Interfaces;
using CSharpFunctionalExtensions;
using PetHome.Domain.Shared.Error;
using PetHome.Domain.PetManagment.VolunteerEntity;
using Microsoft.Extensions.Logging;

namespace PetHome.Application.Features.Volunteers.SoftDeleteVolunteer;
public class SoftDeleteVolunteerUseCase
{
    private readonly IVolunteerRepository _volunteerRepository;
    private readonly ILogger<SoftDeleteVolunteerUseCase> _logger;

    public SoftDeleteVolunteerUseCase(
        IVolunteerRepository volunteerRepository,
        ILogger<SoftDeleteVolunteerUseCase> logger)
    {
        _volunteerRepository = volunteerRepository;
        _logger = logger;
    }

    public async Task<Result<Guid,Error>> Execute(
        Guid id, CancellationToken cancellationToken)
    {
        Volunteer volunteer = _volunteerRepository.GetById(id).Result.Value;
        volunteer.SoftDelete();
        await _volunteerRepository.Update(volunteer, cancellationToken);

        _logger.LogInformation("Волонтёр с id = {0} и его сущности soft deleted", id);

        return id;
    }
}
