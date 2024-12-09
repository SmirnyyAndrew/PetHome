using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using PetHome.Domain.Shared.Interfaces;
using CSharpFunctionalExtensions;
using PetHome.Domain.Shared.Error;
using PetHome.Domain.PetManagment.VolunteerEntity;
using Microsoft.Extensions.Logging;
using PetHome.Application.Interfaces.RepositoryInterfaces;

namespace PetHome.Application.Features.Volunteers.VolunteerManegment.SoftDeleteRestoreVolunteer;
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

    public async Task<Result<Guid, Error>> Execute(
        Guid id, CancellationToken ct)
    {
        Volunteer volunteer = _volunteerRepository.GetById(id, ct).Result.Value;
        volunteer.SoftDelete();
        await _volunteerRepository.Update(volunteer, ct);

        _logger.LogInformation("Волонтёр с id = {0} и его сущности soft deleted", id);

        return id;
    }
}
