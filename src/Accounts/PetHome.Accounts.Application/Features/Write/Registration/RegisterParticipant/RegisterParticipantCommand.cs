using PetHome.Core.Interfaces.FeatureManagment;

namespace PetHome.Accounts.Application.Features.Write.Registration.RegisterParticipant;
public record RegisterParticipantCommand(Guid UserId) : ICommand;

