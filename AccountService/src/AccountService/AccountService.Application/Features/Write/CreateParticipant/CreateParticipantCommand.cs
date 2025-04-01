using PetHome.Core.Interfaces.FeatureManagment;

namespace AccountService.Application.Features.Write.CreateParticipant;
public record CreateParticipantCommand(string Email, string UserName) : ICommand;