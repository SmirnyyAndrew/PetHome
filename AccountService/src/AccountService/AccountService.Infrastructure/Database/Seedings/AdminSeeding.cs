using AccountService.Domain.Accounts;
using AccountService.Domain.Aggregates;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PetHome.Core.Web.Options.Accounts;
using PetHome.SharedKernel.ValueObjects.MainInfo;
using PetHome.SharedKernel.ValueObjects.RolePermission;
using PetHome.SharedKernel.ValueObjects.User;

namespace AccountService.Infrastructure.Database.Seedings;
public static class AdminSeeding
{
    public static IServiceCollection SeedAdmins(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        AuthorizationDbContext dbContext = new AuthorizationDbContext();

        var adminIsExist = dbContext.Admins.Count() > 0;
        if (adminIsExist)
            return services;

        var adminOptions = configuration.GetSection(AdminOption.SECTION_NAME).Get<AdminOption>();
        if (adminOptions is null)
            return services;

        Role? role = dbContext.Roles.FirstOrDefault(r => r.Name.ToLower() == AdminAccount.ROLE);
        RoleId roleId = RoleId.Create(role.Id).Value;
        UserName userName = UserName.Create(adminOptions.UserName).Value;
        Email email = Email.Create(adminOptions.Email).Value;

        User user = User.Create(email, userName, roleId).Value;
        AdminAccount admin = AdminAccount.Create(user).Value;

        dbContext.Users.Add(user);
        dbContext.Admins.Add(admin);
        dbContext.SaveChanges();

        return services;
    }
}
