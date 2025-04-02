using PetHome.Core.Application.Interfaces.FeatureManagement;

namespace AccountService.Contracts.HttpCommunication.Requests.TokenManagement;
public record GenerateRefreshTokenCommand(Guid UserId, string AccessToken) : ICommand;
