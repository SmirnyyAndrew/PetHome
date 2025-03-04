namespace PetHome.Accounts.Contracts.Messaging.UserManagment;
public record ConfirmedUserEmailEvent(Guid UserId, string Email, string UserName, string EmailConfirmationLink);