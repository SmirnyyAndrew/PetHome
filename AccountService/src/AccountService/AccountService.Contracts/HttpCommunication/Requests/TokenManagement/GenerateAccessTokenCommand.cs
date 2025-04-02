using PetHome.Core.Application.Interfaces.FeatureManagement;

namespace AccountService.Contracts.HttpCommunication.Requests.TokenManagement;
public record GenerateAccessTokenCommand(Guid UserId) : ICommand;
