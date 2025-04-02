using PetHome.Core.Application.Interfaces.FeatureManagement;

namespace PetHome.VolunteerRequests.Application.Features.Write.CreateVolunteerRequest;
public record CreateVolunteerRequestCommand(Guid UserId, string VolunteerInfo) : ICommand; 