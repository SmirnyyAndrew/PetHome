using PetHome.Accounts.Application.Features.Write.RegisterAccount;

namespace PetHome.Accounts.API.Controllers.Requests;
public record RegisterParticipantUserRequest(string Email, string Name, string Password)
{
    public static implicit operator RegisterParticipantUserCommand(RegisterParticipantUserRequest request)
    {
        return new RegisterParticipantUserCommand(request.Email, request.Name, request.Password);
    }
}
