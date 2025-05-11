using AccountService.Application.Features.Write.LoginUser;

namespace AccountService.API.Controllers.Requests.Auth;

public record LoginUserRequest(string Email, string Password)
{
    public static implicit operator LoginUserQuery(LoginUserRequest request)
    {
        return new LoginUserQuery(request.Email, request.Password);
    }
}

