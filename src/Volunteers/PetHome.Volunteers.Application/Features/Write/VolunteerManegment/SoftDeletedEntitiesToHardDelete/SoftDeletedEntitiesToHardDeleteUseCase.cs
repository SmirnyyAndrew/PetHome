using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using PetHome.Volunteers.Application.Database.RepositoryInterfaces;
using PetHome.Volunteers.Domain.PetManagment.VolunteerEntity;

namespace PetHome.Volunteers.Application.Features.Write.VolunteerManegment.SoftDeletedEntitiesToHardDelete;
public class SoftDeletedEntitiesToHardDeleteUseCase
{
    private readonly IVolunteerRepository _volunteerRepository;
    private readonly ILogger<SoftDeletedEntitiesToHardDeleteUseCase> _logger;
    public SoftDeletedEntitiesToHardDeleteUseCase(
        IVolunteerRepository volunteerRepository,
        ILogger<SoftDeletedEntitiesToHardDeleteUseCase> logger
        )
    {
        _volunteerRepository = volunteerRepository;
        _logger = logger;
    }

    public Result<int, ErrorList> Execute(CancellationToken ct)
    {
        List<Volunteer> volunteersToHardDelete = _volunteerRepository
            .GetDeleted(ct)
            .Where(x => x.DeletionDate != default(DateTime) && (DateTime.UtcNow - x.DeletionDate).Days > 30)
            .ToList();
        if (volunteersToHardDelete.Count == 0)
            return 0;

        _volunteerRepository.Remove(volunteersToHardDelete);

        int deletedCount = volunteersToHardDelete.Count;
        _logger.LogInformation("Было hard deleted {0} волонётра(-ов)", deletedCount);

        return deletedCount;
    }
}
