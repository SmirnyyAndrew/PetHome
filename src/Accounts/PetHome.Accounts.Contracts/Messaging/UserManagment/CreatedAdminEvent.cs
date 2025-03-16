namespace PetHome.Accounts.Contracts.Messaging.UserManagment;
public record CreatedAdminEvent(Guid UserId, string Email, string UserName);
