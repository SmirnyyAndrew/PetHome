using PetHome.Core.Application.Interfaces.FeatureManagement;

namespace AccountService.Application.Features.Write.EmailManagement.ConfirmEmail;
public record ConfirmEmailCommand(Guid UserId, string Token) : ICommand;