using PetHome.Accounts.Application.Features.Write.EmailManagement.ConfirmEmail;

namespace PetHome.Accounts.API.Controllers.Requests.EmailManagement;
public record ConfirmEmailRequest(Guid UserId, string Token)
{
    public static implicit operator ConfirmEmailCommand(ConfirmEmailRequest request)
    => new(request.UserId, request.Token);
}
