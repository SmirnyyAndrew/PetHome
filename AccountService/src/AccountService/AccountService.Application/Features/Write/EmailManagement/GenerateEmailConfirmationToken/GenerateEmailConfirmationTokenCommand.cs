using PetHome.Core.Application.Interfaces.FeatureManagement;

namespace AccountService.Application.Features.Write.EmailManagement.GenerateEmailConfirmationToken;
public record GenerateEmailConfirmationTokenCommand(Guid UserId) : ICommand;
