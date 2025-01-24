using PetHome.Core.Interfaces.FeatureManagment;

namespace PetHome.Accounts.Application.Features.Write.RegisterAccount;
public record RegisterParticipantUserCommand(string Email, string Name, string Password) : ICommand;
