namespace PetHome.Accounts.Contracts.Messaging.UserManagment;
public record CreatedUserEvent(Guid UserId, string Email, string UserName, Guid RoleId = default);