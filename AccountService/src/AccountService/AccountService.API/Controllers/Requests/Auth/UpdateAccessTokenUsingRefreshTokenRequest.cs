using AccountService.Application.Features.Write.UpdateAccessTokenUsingRefreshToken;

namespace AccountService.API.Controllers.Requests.Auth;
public record UpdateAccessTokenUsingRefreshTokenRequest(Guid RefreshToken)
{
    public static implicit operator UpdateAccessTokenUsingRefreshTokenCommand(
        UpdateAccessTokenUsingRefreshTokenRequest request)
    {
        return new UpdateAccessTokenUsingRefreshTokenCommand(request.RefreshToken);
    }
}
