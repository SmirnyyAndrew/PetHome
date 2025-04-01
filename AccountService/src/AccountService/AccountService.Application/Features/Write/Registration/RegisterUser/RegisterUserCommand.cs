using PetHome.Core.Interfaces.FeatureManagment;

namespace AccountService.Application.Features.Write.Registration.RegisterUser;
public record RegisterUserCommand(string Email, string UserName, string Password) : ICommand;
