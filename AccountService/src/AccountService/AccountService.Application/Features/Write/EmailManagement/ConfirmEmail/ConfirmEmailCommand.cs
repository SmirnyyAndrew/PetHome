using PetHome.Core.Interfaces.FeatureManagment;

namespace AccountService.Application.Features.Write.EmailManagement.ConfirmEmail;
public record ConfirmEmailCommand(Guid UserId, string Token) : ICommand;