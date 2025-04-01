using PetHome.Core.Interfaces.FeatureManagment;

namespace AccountService.Contracts.HttpCommunication.Requests.UserManagement.RolePermissionsManagement;
public record GetUserPermissionsCodesQuery(Guid UserId) : IQuery;
