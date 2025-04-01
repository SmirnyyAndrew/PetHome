using PetHome.Core.Interfaces.FeatureManagment;

namespace AccountService.Application.Features.Write.Registration.RegisterParticipant;
public record RegisterParticipantCommand(Guid UserId) : ICommand;

