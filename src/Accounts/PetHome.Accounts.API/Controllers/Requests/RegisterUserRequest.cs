using PetHome.Accounts.Application.Features.RegisterAccount;

namespace PetHome.Accounts.API.Controllers.Requests;
public record RegisterUserRequest(string Email, string Name, string Password)
{
    public static implicit operator RegisterUserCommand(RegisterUserRequest request)
    {
        return new RegisterUserCommand(request.Email, request.Name, request.Password);
    }
}
