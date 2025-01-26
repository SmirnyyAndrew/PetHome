using PetHome.Accounts.Domain.Aggregates;
using PetHome.Core.ValueObjects.MainInfo;
using PetHome.Core.ValueObjects.User;
namespace PetHome.Accounts.IntegrationTests.Seeds;
public partial class SeedManager
{
    public async Task<IReadOnlyList<User>> SeedUsers(int userCountToSeed = 5)
    {
        List<User> result = new List<User>();

        for (int i = 0; i < userCountToSeed; i++)
        {
            Email email = Email.Create($"emai21l{i}@mail.ru").Value;
            UserName userName = UserName.Create($"Ivanov {i}").Value;
            Role role = _dbContext.Roles.ToList().FirstOrDefault();
            User user = User.Create(email, userName, role).Value;

            result.Add(user);
        }

        await _dbContext.Users.AddRangeAsync(result);
        await _dbContext.SaveChangesAsync();
        return result;
    }
}
