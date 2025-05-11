using AccountService.Application.Features.Write.Registration.RegisterUser;

namespace AccountService.API.Controllers.Requests.Auth;
public record RegisterUserRequest(string Email, string Name, string Password)
{
    public static implicit operator RegisterUserCommand(RegisterUserRequest request)
    {
        return new RegisterUserCommand(request.Email, request.Name, request.Password);
    }
}
