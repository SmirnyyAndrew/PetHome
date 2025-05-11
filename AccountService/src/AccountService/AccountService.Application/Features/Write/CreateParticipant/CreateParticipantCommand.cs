using PetHome.Core.Application.Interfaces.FeatureManagement;

namespace AccountService.Application.Features.Write.CreateParticipant;
public record CreateParticipantCommand(string Email, string UserName) : ICommand;