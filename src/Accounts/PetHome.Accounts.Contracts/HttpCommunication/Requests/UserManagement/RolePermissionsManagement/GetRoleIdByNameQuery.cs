using PetHome.Core.Interfaces.FeatureManagment;

namespace PetHome.Accounts.Contracts.HttpCommunication.Requests.UserManagement.RolePermissionsManagement;
public record GetRoleIdByNameQuery(string Name) : IQuery;
