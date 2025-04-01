using PetHome.Core.Interfaces.FeatureManagment;

namespace AccountService.Contracts.HttpCommunication.Requests.UserManagement;
public record GetUserMainInformationQuery(Guid UserId) : IQuery;
