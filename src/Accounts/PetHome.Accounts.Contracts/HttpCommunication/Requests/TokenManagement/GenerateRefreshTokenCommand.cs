using PetHome.Core.Interfaces.FeatureManagment;

namespace PetHome.Accounts.Contracts.HttpCommunication.Requests.TokenManagement;
public record GenerateRefreshTokenCommand(Guid UserId, string AccessToken) : ICommand;
