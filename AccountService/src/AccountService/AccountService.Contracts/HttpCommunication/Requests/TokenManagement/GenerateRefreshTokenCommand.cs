using PetHome.Core.Interfaces.FeatureManagment;

namespace AccountService.Contracts.HttpCommunication.Requests.TokenManagement;
public record GenerateRefreshTokenCommand(Guid UserId, string AccessToken) : ICommand;
