using PetHome.Accounts.Application.Features.LoginAccount;

namespace PetHome.Accounts.API.Controllers.Requests;

public record LoginAccountRequest(string Email, string Password)
{
    public static implicit operator LoginAccountQuery(LoginAccountRequest request)
    {
        return new LoginAccountQuery(request.Email, request.Password);
    }
}

