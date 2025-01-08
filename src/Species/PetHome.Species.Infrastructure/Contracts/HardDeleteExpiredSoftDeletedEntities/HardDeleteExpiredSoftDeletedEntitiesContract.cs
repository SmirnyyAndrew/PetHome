using Microsoft.Extensions.Configuration;
using PetHome.Core.Extentions.Collection;
using PetHome.Core.Interfaces.FeatureManagment;
using PetHome.SharedKernel.Options.Backgroundd;
using PetHome.Species.Application.Database;
using PetHome.Species.Infrastructure.Database.Write.DBContext;

namespace PetHome.Species.Infrastructure.Contracts.HardDeleteExpiredSoftDeletedEntities;
public class HardDeleteExpiredSoftDeletedEntitiesContract
    : IHardDeleteSoftDeletedEntitiesContract
{
    private readonly SoftDeletableEntitiesOption _option;
    private readonly SpeciesWriteDbContext _dbContext;

    public HardDeleteExpiredSoftDeletedEntitiesContract(
        IConfiguration configuration,
        SpeciesWriteDbContext dbContext)
    {
        _option = configuration.GetSection(SoftDeletableEntitiesOption.SECTION_NAME).Get<SoftDeletableEntitiesOption>();
        _dbContext = dbContext;
    }

    public async Task HardDeleteExpiredSoftDeletedEntities(CancellationToken ct)
    {
        var speciesToDelete = _dbContext.Species
            .GetExpiredEntitiesList(_option.DaysToHardDelete);
        _dbContext.Species.RemoveRange(speciesToDelete);
        await _dbContext.SaveChangesAsync(ct);
    }
}


