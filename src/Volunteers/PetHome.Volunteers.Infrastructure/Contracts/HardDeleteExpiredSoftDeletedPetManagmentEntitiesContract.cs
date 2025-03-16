using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PetHome.Core.Extentions.Collection;
using PetHome.Core.Interfaces.FeatureManagment;
using PetHome.SharedKernel.Options.Background;
using PetHome.Species.Infrastructure.Contracts.HardDeleteExpiredSoftDeletedEntities;
using PetHome.Volunteers.Infrastructure.Database.Write.DBContext;

namespace PetHome.Volunteers.Infrastructure.Contracts;
public class HardDeleteExpiredSoftDeletedPetManagmentEntitiesContract
    : IHardDeleteSoftDeletedEntitiesContract
{
    private readonly SoftDeletableEntitiesOption _option;
    private readonly VolunteerWriteDbContext _dbContext;
    private readonly ILogger<HardDeleteExpiredSoftDeletedSpeciesEntitiesContract> _logger;

    public HardDeleteExpiredSoftDeletedPetManagmentEntitiesContract(
        IConfiguration configuration,
        VolunteerWriteDbContext dbContext,
        ILogger<HardDeleteExpiredSoftDeletedSpeciesEntitiesContract> logger)
    {
        _option = configuration.GetSection(SoftDeletableEntitiesOption.SECTION_NAME).Get<SoftDeletableEntitiesOption>();
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task HardDeleteExpiredSoftDeletedEntities(CancellationToken ct)
    {
        var volunteersToDelete = _dbContext.Volunteers?
            .GetExpiredEntitiesList(_option.DaysToHardDelete);
        _dbContext.Volunteers?.RemoveRange(volunteersToDelete);
        await _dbContext.SaveChangesAsync(ct);

        LogInformationAboutRemovingEntities(nameof(HardDeleteExpiredSoftDeletedSpeciesEntitiesContract), "Volunteers", volunteersToDelete?.Count);
    }

    private void LogInformationAboutRemovingEntities(string className, string entityName, int? count)
    {
        string loggMessage = $"{className}: удалено {count} {entityName}";
        _logger.LogInformation(loggMessage);
    }
}


