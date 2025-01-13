using PetHome.Core.Interfaces.FeatureManagment;

namespace PetHome.VolunteerRequests.Application.Features.Write.CreateVolunteerRequests;
public record CreateVolunteerRequestsCommand(Guid UserId, string VolunteerInfo) : ICommand; 