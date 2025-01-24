using PetHome.Core.Interfaces.FeatureManagment;
using PetHome.VolunteerRequests.Domain;

namespace PetHome.VolunteerRequests.Application.Features.Read.GetAllAdminVolunteerRequests;
public record GetAllAdminVolunteerRequestsQuery(
    Guid AdminId,
    int PageSize,
    int PageNum,
    VolunteerRequestStatus? Status) : IQuery;
