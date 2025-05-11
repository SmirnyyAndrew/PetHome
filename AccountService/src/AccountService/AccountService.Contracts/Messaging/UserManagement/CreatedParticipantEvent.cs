namespace AccountService.Contracts.Messaging.UserManagement;
public record CreatedParticipantEvent(Guid UserId, string Email, string UserName);
