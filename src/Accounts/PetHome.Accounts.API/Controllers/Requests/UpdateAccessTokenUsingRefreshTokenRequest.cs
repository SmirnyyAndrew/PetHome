using PetHome.Accounts.Application.Features.Write.UpdateAccessTokenUsingRefreshToken;

namespace PetHome.Accounts.API.Controllers.Requests;
public record UpdateAccessTokenUsingRefreshTokenRequest(Guid RefreshToken)
{
    public static implicit operator UpdateAccessTokenUsingRefreshTokenCommand(
        UpdateAccessTokenUsingRefreshTokenRequest request)
    {
        return new UpdateAccessTokenUsingRefreshTokenCommand(request.RefreshToken);
    }
}
