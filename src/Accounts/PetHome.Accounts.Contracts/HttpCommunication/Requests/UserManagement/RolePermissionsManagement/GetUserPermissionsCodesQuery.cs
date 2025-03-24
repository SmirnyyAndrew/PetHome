using PetHome.Core.Interfaces.FeatureManagment;

namespace PetHome.Accounts.Contracts.HttpCommunication.Requests.UserManagement.RolePermissionsManagement;
public record GetUserPermissionsCodesQuery(Guid UserId) : IQuery;
