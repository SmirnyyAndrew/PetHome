using PetHome.Core.Interfaces.FeatureManagment;

namespace PetHome.VolunteerRequests.Application.Features.Write.SetVolunteerRequestApproved;
public record SetVolunteerRequestApprovedCommand(Guid VolunteerRequestId, Guid AdminId) : ICommand;
