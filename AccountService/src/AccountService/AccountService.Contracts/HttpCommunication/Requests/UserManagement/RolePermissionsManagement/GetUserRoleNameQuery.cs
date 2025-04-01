using PetHome.Core.Interfaces.FeatureManagment;

namespace AccountService.Contracts.HttpCommunication.Requests.UserManagement.RolePermissionsManagement;
public record GetUserRoleNameQuery(Guid UserId) : IQuery;
