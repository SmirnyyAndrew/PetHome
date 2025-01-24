using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PetHome.Accounts.Domain.Accounts;
using PetHome.Accounts.Domain.Aggregates;
using PetHome.Core.ValueObjects.MainInfo;
using PetHome.Core.ValueObjects.User;
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
        if(adminOptions is null)
            return services;

        Role? role = dbContext.Roles.FirstOrDefault(r => r.Name.ToLower() == AdminAccount.ROLE);  
        UserName userName = UserName.Create(adminOptions.UserName).Value;
        Email email = Email.Create(adminOptions.Email).Value;

        User user = User.Create(email, userName, role).Value;
        AdminAccount admin = AdminAccount.Create(user).Value;
        
        dbContext.Users.Add(user);
        dbContext.Admins.Add(admin);
        dbContext.SaveChanges();

        return services;
    }
}
