using PetHome.Core.Interfaces.FeatureManagment;

namespace PetHome.VolunteerRequests.Application.Features.Write.SetVolunteerRequestSubmitted;
public record SetVolunteerRequestSubmittedCommand(Guid VolunteerRequestId, Guid AdminId) : ICommand;
