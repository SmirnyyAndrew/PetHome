namespace PetHome.Accounts.Contracts.Messaging.UserManagment;
public record CreatedParticipantEvent(Guid UserId, string Email, string UserName);
