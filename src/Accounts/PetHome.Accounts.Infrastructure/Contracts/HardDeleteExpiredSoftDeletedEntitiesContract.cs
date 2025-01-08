using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using PetHome.Accounts.Infrastructure.Database;
using PetHome.Core.Extentions.Collection;
using PetHome.Core.Interfaces.FeatureManagment;
using PetHome.SharedKernel.Options.Backgroundd;

namespace PetHome.Accounts.Infrastructure.Contracts;
public class HardDeleteExpiredSoftDeletedEntitiesContract
    : IHardDeleteSoftDeletedEntitiesContract
{
    private readonly SoftDeletableEntitiesOption _option;
    private readonly AuthorizationDbContext _dbContext;

    public HardDeleteExpiredSoftDeletedEntitiesContract(
        IConfiguration configuration,
        AuthorizationDbContext dbContext)
    {
        _option = configuration.GetSection(SoftDeletableEntitiesOption.SECTION_NAME).Get<SoftDeletableEntitiesOption>();
        _dbContext = dbContext;
    }

    public async Task HardDeleteExpiredSoftDeletedEntities(CancellationToken ct)
    {
        await Task.WhenAll(
            HardDeleteUsers(ct),
            HardDeleteAdmins(ct),
            HardDeleteParticipants(ct),
            HardDeleteVolunteers(ct));
    }

    private async Task HardDeleteUsers(CancellationToken ct)
    {
        var usersToDelete = _dbContext.Users
            .GetExpiredEntitiesList(_option.DaysToHardDelete);
        _dbContext.Users.RemoveRange(usersToDelete);
        await _dbContext.SaveChangesAsync(ct);
    }

    private async Task HardDeleteAdmins(CancellationToken ct)
    {
        var adminsToDelete = await _dbContext.Admins
            .Where(s => s.DeletionDate.AddDays(_option.DaysToHardDelete) >= DateTime.UtcNow)
            .ToListAsync(ct);
        _dbContext.Admins.RemoveRange(adminsToDelete);
        await _dbContext.SaveChangesAsync(ct);
    }

    private async Task HardDeleteParticipants(CancellationToken ct)
    {
        var participantsToDelete = await _dbContext.ParticipantAccounts
            .Where(s => s.DeletionDate.AddDays(_option.DaysToHardDelete) >= DateTime.UtcNow)
            .ToListAsync(ct);
        _dbContext.ParticipantAccounts.RemoveRange(participantsToDelete);
        await _dbContext.SaveChangesAsync(ct);
    }

    private async Task HardDeleteVolunteers(CancellationToken ct)
    {
        var volunteersToDelete = await _dbContext.VolunteerAccounts
            .Where(s => s.DeletionDate.AddDays(_option.DaysToHardDelete) >= DateTime.UtcNow)
            .ToListAsync(ct);
        _dbContext.VolunteerAccounts.RemoveRange(volunteersToDelete);
        await _dbContext.SaveChangesAsync(ct);
    }
}


