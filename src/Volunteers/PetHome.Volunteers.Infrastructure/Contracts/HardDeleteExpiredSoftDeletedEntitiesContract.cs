using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using PetHome.Core.Extentions.Collection;
using PetHome.Core.Interfaces.FeatureManagment;
using PetHome.SharedKernel.Options.Backgroundd;
using PetHome.Volunteers.Application.Database;
using PetHome.Volunteers.Infrastructure.Database.Write.DBContext;

namespace PetHome.Volunteers.Infrastructure.Contracts;
public class HardDeleteExpiredSoftDeletedEntitiesContract
    : IHardDeleteSoftDeletedEntitiesContract
{
    private readonly SoftDeletableEntitiesOption _option;
    private readonly VolunteerWriteDbContext _dbContext;

    public HardDeleteExpiredSoftDeletedEntitiesContract(
        IConfiguration configuration,
        VolunteerWriteDbContext dbContext)
    {
        _option = configuration.GetSection(SoftDeletableEntitiesOption.SECTION_NAME).Get<SoftDeletableEntitiesOption>();
        _dbContext = dbContext;
    }

    public async Task HardDeleteExpiredSoftDeletedEntities(CancellationToken ct)
    {
        var volunteersToDelete = _dbContext.Volunteers
            .GetExpiredEntitiesList(_option.DaysToHardDelete);
        _dbContext.Volunteers.RemoveRange(volunteersToDelete);
        await _dbContext.SaveChangesAsync(ct);
    }
}


