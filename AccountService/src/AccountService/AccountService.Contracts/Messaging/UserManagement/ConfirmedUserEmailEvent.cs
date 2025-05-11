namespace AccountService.Contracts.Messaging.UserManagement;
public record ConfirmedUserEmailEvent(Guid UserId, string Email, string UserName, string EmailConfirmationLink);