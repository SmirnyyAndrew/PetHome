using PetHome.Core.Application.Interfaces.FeatureManagement;

namespace AccountService.Application.Features.Write.Registration.RegisterUser;
public record RegisterUserCommand(string Email, string UserName, string Password) : ICommand;
