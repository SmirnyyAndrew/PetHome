using PetHome.Accounts.Application.Features.LoginAccount;

namespace PetHome.Accounts.API.Controllers.Requests;

public record LoginAccountRequest(string Login, string Password)
{
    public static implicit operator LoginAccountQuery(LoginAccountRequest request)
    {
        return new LoginAccountQuery(request.Login, request.Password);
    }
}

