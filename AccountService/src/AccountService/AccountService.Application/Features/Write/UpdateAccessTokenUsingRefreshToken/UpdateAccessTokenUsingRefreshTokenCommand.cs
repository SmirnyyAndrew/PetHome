using PetHome.Core.Application.Interfaces.FeatureManagement;

namespace AccountService.Application.Features.Write.UpdateAccessTokenUsingRefreshToken;
public record UpdateAccessTokenUsingRefreshTokenCommand(Guid RefreshToken) : ICommand;
