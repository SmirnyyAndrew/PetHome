namespace AccountService.Contracts.Messaging.UserManagement;
public record CreatedAdminEvent(Guid UserId, string Email, string UserName);
