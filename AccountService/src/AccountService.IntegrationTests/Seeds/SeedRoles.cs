using AccountService.Domain.Accounts;
using AccountService.Domain.Aggregates;
namespace AccountService.IntegrationTests.Seeds;
public partial class SeedManager
{
    public async Task<IReadOnlyList<Role>> SeedRoles()
    {
        List<Role> result = new();

        if (_dbContext.Roles.Count() == 0)
        {
            result = new List<Role>()
                {
                    Role.Create(AdminAccount.ROLE).Value,
                    Role.Create(ParticipantAccount.ROLE).Value,
                    Role.Create(VolunteerAccount.ROLE).Value,
                    Role.Create("visitor").Value,
                };

            await _dbContext.Roles.AddRangeAsync(result);
            await _dbContext.SaveChangesAsync();
            return result;
        }
        return result;
    }
}
