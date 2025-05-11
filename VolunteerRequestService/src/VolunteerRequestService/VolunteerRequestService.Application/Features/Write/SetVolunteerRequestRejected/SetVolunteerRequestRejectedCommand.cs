using PetHome.Core.Application.Interfaces.FeatureManagement;

namespace PetHome.VolunteerRequests.Application.Features.Write.SetVolunteerRequestRejected;
public record SetVolunteerRequestRejectedCommand(
    Guid VolunteerRequestId, 
    Guid AdminId, 
    string RejectedComment) : ICommand;
