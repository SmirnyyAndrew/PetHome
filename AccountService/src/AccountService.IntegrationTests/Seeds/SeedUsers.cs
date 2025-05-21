using AccountService.Domain.Aggregates;
using PetHome.SharedKernel.ValueObjects.MainInfo;
using PetHome.SharedKernel.ValueObjects.RolePermission;
using PetHome.SharedKernel.ValueObjects.User;
namespace AccountService.IntegrationTests.Seeds;
public partial class SeedManager
{
    public async Task<IReadOnlyList<User>> SeedUsers(int userCountToSeed)
    {
        List<User> result = new List<User>();

        for (int i = 0; i < userCountToSeed; i++)
        {
            Email email = Email.Create($"emai21l{i}@mail.ru").Value;
            UserName userName = UserName.Create($"Ivanov {i}").Value;
            Role role = _dbContext.Roles.ToList().FirstOrDefault(); 
            RoleId roleId = RoleId.Create(role.Id).Value;

            User user = User.Create(email, userName, roleId).Value;

            result.Add(user);
        }

        await _dbContext.Users.AddRangeAsync(result);
        await _dbContext.SaveChangesAsync();
        return result;
    }
}
