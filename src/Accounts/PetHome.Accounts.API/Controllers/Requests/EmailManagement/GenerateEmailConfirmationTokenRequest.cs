using PetHome.Accounts.Application.Features.Write.EmailManagement.GenerateEmailConfirmationToken;

namespace PetHome.Accounts.API.Controllers.Requests.EmailManagement;
public record GenerateEmailConfirmationTokenRequest(Guid UserId)
{
    public static implicit operator GenerateEmailConfirmationTokenCommand(GenerateEmailConfirmationTokenRequest request)
    => new(request.UserId);
}
