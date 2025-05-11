using AccountService.Application.Features.Write.EmailManagement.ConfirmEmail;

namespace AccountService.API.Controllers.Requests.EmailManagement;
public record ConfirmEmailRequest(Guid UserId, string Token)
{
    public static implicit operator ConfirmEmailCommand(ConfirmEmailRequest request)
    => new(request.UserId, request.Token);
}
