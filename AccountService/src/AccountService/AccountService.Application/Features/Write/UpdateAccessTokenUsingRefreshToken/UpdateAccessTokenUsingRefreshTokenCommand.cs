using PetHome.Core.Interfaces.FeatureManagment;

namespace AccountService.Application.Features.Write.UpdateAccessTokenUsingRefreshToken;
public record UpdateAccessTokenUsingRefreshTokenCommand(Guid RefreshToken) : ICommand;
