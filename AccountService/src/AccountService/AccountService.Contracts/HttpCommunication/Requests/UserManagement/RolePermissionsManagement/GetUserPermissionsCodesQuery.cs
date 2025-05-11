using PetHome.Core.Application.Interfaces.FeatureManagement;

namespace AccountService.Contracts.HttpCommunication.Requests.UserManagement.RolePermissionsManagement;
public record GetUserPermissionsCodesQuery(Guid UserId) : IQuery;
