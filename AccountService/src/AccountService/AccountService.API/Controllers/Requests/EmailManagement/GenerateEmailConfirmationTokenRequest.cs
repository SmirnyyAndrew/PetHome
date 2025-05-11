using AccountService.Application.Features.Write.EmailManagement.GenerateEmailConfirmationToken;

namespace AccountService.API.Controllers.Requests.EmailManagement;
public record GenerateEmailConfirmationTokenRequest(Guid UserId)
{
    public static implicit operator GenerateEmailConfirmationTokenCommand(GenerateEmailConfirmationTokenRequest request)
    => new(request.UserId);
}
