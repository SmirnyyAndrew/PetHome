using PetHome.Core.Interfaces.FeatureManagment;

namespace AccountService.Contracts.HttpCommunication.Requests.TokenManagement;
public record GenerateAccessTokenCommand(Guid UserId) : ICommand;
