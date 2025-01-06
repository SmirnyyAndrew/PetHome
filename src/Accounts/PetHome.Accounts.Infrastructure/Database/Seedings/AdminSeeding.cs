using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PetHome.Accounts.Domain.Aggregates.RolePermission;
using PetHome.Accounts.Domain.Aggregates.User;
using PetHome.Accounts.Domain.Aggregates.User.Accounts;
using PetHome.Core.ValueObjects;
using PetHome.SharedKernel.Options.Accounts;

namespace PetHome.Accounts.Infrastructure.Database.Seedings;
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

        Role? role = dbContext.Roles.FirstOrDefault(r => r.Name.ToLower() == AdminAccount.ROLE); 
        UserName userName = UserName.Create(adminOptions.UserName).Value;
        Email email = Email.Create(adminOptions.Email).Value;

        User user = User.Create(email, userName, role);
        AdminAccount admin = AdminAccount.Create(user).Value;
        
        dbContext.Users.Add(user);
        dbContext.Admins.Add(admin);
        dbContext.SaveChanges();

        return services;
    }
}
