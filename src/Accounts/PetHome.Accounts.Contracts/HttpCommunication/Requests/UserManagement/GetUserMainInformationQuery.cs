using PetHome.Core.Interfaces.FeatureManagment;

namespace PetHome.Accounts.Contracts.HttpCommunication.Requests.UserManagement;
public record GetUserMainInformationQuery(Guid UserId) : IQuery;
