using PetHome.Core.Application.Interfaces.FeatureManagement;

namespace PetHome.VolunteerRequests.Application.Features.Read.GetAllUserVolunteerRequests;
public record GetAllUserVolunteerRequestsQuery(Guid UserId, int PageSize, int PageNum) : IQuery;
