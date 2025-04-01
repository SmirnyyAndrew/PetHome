using PetHome.Core.Interfaces.FeatureManagment;

namespace PetHome.VolunteerRequests.Application.Features.Write.CreateVolunteerRequest;
public record CreateVolunteerRequestCommand(Guid UserId, string VolunteerInfo) : ICommand; 