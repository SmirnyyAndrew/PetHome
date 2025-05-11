using PetHome.Core.Application.Interfaces.FeatureManagement;

namespace AccountService.Application.Features.Write.CreateUser;
public record CreateUserCommand(Guid RoleId, string Email, string UserName) : ICommand;
