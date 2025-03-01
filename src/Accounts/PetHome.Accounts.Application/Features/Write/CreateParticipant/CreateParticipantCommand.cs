using PetHome.Core.Interfaces.FeatureManagment;

namespace PetHome.Accounts.Application.Features.Write.CreateParticipant;
public record CreateParticipantCommand(string Email, string UserName) : ICommand;