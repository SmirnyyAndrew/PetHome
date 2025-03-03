using PetHome.Core.Interfaces.FeatureManagment;

namespace PetHome.Accounts.Application.Features.Write.Registration.RegisterAccount;
public record RegisterUserCommand(string Email, string UserName, string Password) : ICommand;
