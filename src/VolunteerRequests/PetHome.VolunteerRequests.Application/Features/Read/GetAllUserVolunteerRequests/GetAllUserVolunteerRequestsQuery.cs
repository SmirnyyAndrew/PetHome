using PetHome.Core.Interfaces.FeatureManagment;

namespace PetHome.VolunteerRequests.Application.Features.Read.GetAllUserVolunteerRequests;
public record GetAllUserVolunteerRequestsQuery(Guid UserId) : IQuery;
