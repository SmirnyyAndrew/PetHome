using PetHome.Core.Interfaces.FeatureManagment;

namespace PetHome.Accounts.Application.Features.RegisterAccount;
public record RegisterUserCommand(string Email, string Name, string Password) : ICommand;
