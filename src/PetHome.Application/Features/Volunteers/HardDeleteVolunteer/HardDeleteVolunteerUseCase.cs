using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using PetHome.Application.Features.Volunteers.RepositoryInterfaces;
using PetHome.Domain.PetManagment.VolunteerEntity;
using PetHome.Domain.Shared.Error;

namespace PetHome.Application.Features.Volunteers.HardDeleteVolunteer;
public class HardDeleteVolunteerUseCase
{
    private readonly IVolunteerRepository _volunteerRepository;
    private readonly ILogger<HardDeleteVolunteerUseCase> _loger;

    public HardDeleteVolunteerUseCase(
        IVolunteerRepository volunteerRepository,
        ILogger<HardDeleteVolunteerUseCase> logger)
    {
        _volunteerRepository = volunteerRepository;
        _loger = logger;
    }

    public async Task<Result<bool, Error>> Execute(
        VolunteerId id, CancellationToken ct)
    {
        bool hasDeleted = _volunteerRepository.RemoveById(id, ct).Result;

        _loger.LogInformation("Волонтёр с id = {0} навсегда удалён", id);

        return hasDeleted;
    }


}
