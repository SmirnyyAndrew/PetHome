using PetHome.Core.Interfaces.FeatureManagment;

namespace PetHome.VolunteerRequests.Application.Features.Write.SetVolunteerRequestRejected;
public record SetVolunteerRequestRejectedCommand(
    Guid VolunteerRequestId, 
    Guid AdminId, 
    string RejectedComment) : ICommand;
