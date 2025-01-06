using PetHome.Accounts.Application.Features.LoginAccount;

namespace PetHome.Accounts.API.Controllers.Requests;

public record LoginUserRequest(string Email, string Password)
{
    public static implicit operator LoginUserQuery(LoginUserRequest request)
    {
        return new LoginUserQuery(request.Email, request.Password);
    }
}

