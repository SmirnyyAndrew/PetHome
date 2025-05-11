namespace AccountService.Contracts.Messaging.UserManagement;
public record CreatedUserEvent(Guid UserId, string Email, string UserName, string RoleName);
