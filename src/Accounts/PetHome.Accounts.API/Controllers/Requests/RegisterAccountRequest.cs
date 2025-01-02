using PetHome.Accounts.Application.Features.RegisterAccount;

namespace PetHome.Accounts.API.Controllers.Requests;
public record RegisterAccountRequest(string Login, string Password)
{
    public static implicit operator RegisterAccountCommand(RegisterAccountRequest request)
    {
        return new RegisterAccountCommand(request.Login, request.Password);
    }
}
