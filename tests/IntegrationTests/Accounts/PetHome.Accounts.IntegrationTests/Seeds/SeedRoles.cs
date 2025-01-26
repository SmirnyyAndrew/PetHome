using PetHome.Accounts.Domain.Aggregates;
namespace PetHome.Accounts.IntegrationTests.Seeds;
public partial class SeedManager
{
    public async Task<IReadOnlyList<Role>> SeedRoles()
    {
        List<Role> result = new();

        if (_dbContext.Roles.Count() == 0)
        {
            result = new List<Role>()
                {
                    Role.Create("admin").Value,
                    Role.Create("participant").Value,
                    Role.Create("volunteer").Value,
                    Role.Create("visitor").Value,
                };

            await _dbContext.Roles.AddRangeAsync(result);
            await _dbContext.SaveChangesAsync();
            return result;
        }
        return result;
    }
}
