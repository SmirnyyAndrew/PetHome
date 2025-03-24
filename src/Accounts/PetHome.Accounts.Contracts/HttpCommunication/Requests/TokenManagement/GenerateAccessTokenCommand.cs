using PetHome.Core.Interfaces.FeatureManagment;

namespace PetHome.Accounts.Contracts.HttpCommunication.Requests.TokenManagement;
public record GenerateAccessTokenCommand(Guid UserId) : ICommand;
