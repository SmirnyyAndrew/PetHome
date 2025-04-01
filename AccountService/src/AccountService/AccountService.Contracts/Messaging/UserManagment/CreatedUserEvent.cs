namespace AccountService.Contracts.Messaging.UserManagment;
public record CreatedUserEvent(Guid UserId, string Email, string UserName, string RoleName);
