using AccountService.Infrastructure.Database;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PetHome.Core.Extentions.Collection;
using PetHome.Core.Interfaces.FeatureManagment;
using PetHome.SharedKernel.Options.Background;

namespace AccountService.Infrastructure.Contracts;
public class HardDeleteExpiredSoftDeletedAccountEntitiesContract
    : IHardDeleteSoftDeletedEntitiesContract
{
    private readonly SoftDeletableEntitiesOption _option;
    private readonly AuthorizationDbContext _dbContext;
    private readonly ILogger<HardDeleteExpiredSoftDeletedAccountEntitiesContract> _logger;

    public HardDeleteExpiredSoftDeletedAccountEntitiesContract(
        IConfiguration configuration,
        AuthorizationDbContext dbContext,
        ILogger<HardDeleteExpiredSoftDeletedAccountEntitiesContract> logger)
    {
        _option = configuration.GetSection(SoftDeletableEntitiesOption.SECTION_NAME).Get<SoftDeletableEntitiesOption>();
        _dbContext = dbContext;
        _logger = logger;
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
        var usersToDelete = _dbContext.Users?
            .GetExpiredEntitiesList(_option.DaysToHardDelete);
        _dbContext.Users?.RemoveRange(usersToDelete);
        await _dbContext.SaveChangesAsync(ct);

        LogInformationAboutRemovingEntities(nameof(HardDeleteExpiredSoftDeletedAccountEntitiesContract), "UserAccounts", usersToDelete?.Count);
    }

    private async Task HardDeleteAdmins(CancellationToken ct)
    {
        var adminsToDelete = _dbContext.Admins?
            .GetExpiredEntitiesList(_option.DaysToHardDelete);
        _dbContext.Admins?.RemoveRange(adminsToDelete);
        await _dbContext.SaveChangesAsync(ct);

        LogInformationAboutRemovingEntities(nameof(HardDeleteExpiredSoftDeletedAccountEntitiesContract), "AdminAccounts", adminsToDelete?.Count);
    }

    private async Task HardDeleteParticipants(CancellationToken ct)
    {
        var participantsToDelete = _dbContext.ParticipantAccounts?
            .GetExpiredEntitiesList(_option.DaysToHardDelete);
        _dbContext.ParticipantAccounts?.RemoveRange(participantsToDelete);
        await _dbContext.SaveChangesAsync(ct);

        LogInformationAboutRemovingEntities(nameof(HardDeleteExpiredSoftDeletedAccountEntitiesContract), "ParticipantAccounts", participantsToDelete?.Count);
    }

    private async Task HardDeleteVolunteers(CancellationToken ct)
    {
        var volunteersToDelete = _dbContext.VolunteerAccounts?
            .GetExpiredEntitiesList(_option.DaysToHardDelete);
        _dbContext.VolunteerAccounts?.RemoveRange(volunteersToDelete);
        await _dbContext.SaveChangesAsync(ct);

        LogInformationAboutRemovingEntities(nameof(HardDeleteExpiredSoftDeletedAccountEntitiesContract), "VolunteerAccounts", volunteersToDelete?.Count);
    }

    private void LogInformationAboutRemovingEntities(string className, string entityName, int? count)
    {
        string loggMessage = $"{className}: удалено {count} {entityName}";
        _logger.LogInformation(loggMessage);
    }
}


