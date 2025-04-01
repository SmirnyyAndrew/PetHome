using PetHome.Core.Interfaces.FeatureManagment;

namespace AccountService.Application.Features.Write.EmailManagement.GenerateEmailConfirmationToken;
public record GenerateEmailConfirmationTokenCommand(Guid UserId) : ICommand;
