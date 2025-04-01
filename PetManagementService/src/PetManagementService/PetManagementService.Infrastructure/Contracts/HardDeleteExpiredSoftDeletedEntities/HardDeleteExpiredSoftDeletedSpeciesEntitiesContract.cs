using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PetHome.Core.Extentions.Collection;
using PetHome.Core.Interfaces.FeatureManagment;
using PetHome.SharedKernel.Options.Background;
using PetManagementService.Infrastructure.Database.Write.DBContext;

namespace PetManagementService.Infrastructure.Contracts.HardDeleteExpiredSoftDeletedEntities;
public class HardDeleteExpiredSoftDeletedSpeciesEntitiesContract
    : IHardDeleteSoftDeletedEntitiesContract
{
    private readonly SoftDeletableEntitiesOption _option;
    private readonly PetManagementWriteDBContext _dbContext;
    private readonly ILogger<HardDeleteExpiredSoftDeletedSpeciesEntitiesContract> _logger;

    public HardDeleteExpiredSoftDeletedSpeciesEntitiesContract(
        IConfiguration configuration,
        PetManagementWriteDBContext dbContext,
        ILogger<HardDeleteExpiredSoftDeletedSpeciesEntitiesContract> logger)
    {
        _option = configuration.GetSection(SoftDeletableEntitiesOption.SECTION_NAME).Get<SoftDeletableEntitiesOption>();
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task HardDeleteExpiredSoftDeletedEntities(CancellationToken ct)
    {
        var speciesToDelete = _dbContext.Species?
            .GetExpiredEntitiesList(_option.DaysToHardDelete);
        _dbContext.Species?.RemoveRange(speciesToDelete);
        await _dbContext.SaveChangesAsync(ct);

        LogInformationAboutRemovingEntities(nameof(HardDeleteExpiredSoftDeletedSpeciesEntitiesContract), "Species", speciesToDelete?.Count);
    }
     
    private void LogInformationAboutRemovingEntities(string className, string entityName, int? count)
    {
        string loggMessage = $"{className}: удалено {count} {entityName}";
        _logger.LogInformation(loggMessage);
    }
}


