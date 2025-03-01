namespace PetHome.Accounts.Contracts.Messaging.UserManagment;
public record CreatedParticipantEvent(Guid Id, string Email, string UserName);
