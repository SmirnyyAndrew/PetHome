namespace PetHome.Accounts.Contracts.Messaging.UserManagment;
public record CreatedUserEvent(string Email, string UserName, Guid RoleId = default);