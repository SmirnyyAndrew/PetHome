using PetHome.Core.Interfaces.FeatureManagment;

namespace PetHome.Accounts.Application.Features.Write.CreateUser;
public record CreateUserCommand(Guid RoleId, string Email, string UserName) : ICommand;
