using PetHome.Core.Application.Interfaces.FeatureManagement;

namespace AccountService.Application.Features.Write.Registration.RegisterParticipant;
public record RegisterParticipantCommand(Guid UserId) : ICommand;

