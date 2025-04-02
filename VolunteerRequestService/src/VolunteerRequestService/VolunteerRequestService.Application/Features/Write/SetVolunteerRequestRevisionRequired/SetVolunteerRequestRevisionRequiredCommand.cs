using PetHome.Core.Application.Interfaces.FeatureManagement;

namespace PetHome.VolunteerRequests.Application.Features.Write.SetVolunteerRequestRevisionRequired;
public record SetVolunteerRequestRevisionRequiredCommand(
    Guid VolunteerRequestId, 
    Guid AdminId, 
    string RejectedComment) : ICommand;
