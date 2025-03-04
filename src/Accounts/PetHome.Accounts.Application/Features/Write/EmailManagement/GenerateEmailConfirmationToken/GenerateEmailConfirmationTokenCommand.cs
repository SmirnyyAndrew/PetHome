using PetHome.Core.Interfaces.FeatureManagment;

namespace PetHome.Accounts.Application.Features.Write.EmailManagement.GenerateEmailConfirmationToken;
public record GenerateEmailConfirmationTokenCommand(Guid UserId) : ICommand;
