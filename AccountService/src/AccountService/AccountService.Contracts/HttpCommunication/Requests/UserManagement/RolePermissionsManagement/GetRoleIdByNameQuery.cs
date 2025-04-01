using PetHome.Core.Interfaces.FeatureManagment;

namespace AccountService.Contracts.HttpCommunication.Requests.UserManagement.RolePermissionsManagement;
public record GetRoleIdByNameQuery(string Name) : IQuery;
