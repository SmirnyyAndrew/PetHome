using PetHome.Core.Application.Interfaces.FeatureManagement;

namespace PetHome.VolunteerRequests.Application.Features.Write.SetVolunteerRequestSubmitted;
public record SetVolunteerRequestSubmittedCommand(Guid VolunteerRequestId, Guid AdminId) : ICommand;
