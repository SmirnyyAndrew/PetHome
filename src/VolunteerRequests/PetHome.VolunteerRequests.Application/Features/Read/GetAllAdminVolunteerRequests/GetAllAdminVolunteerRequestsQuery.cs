using PetHome.Core.Interfaces.FeatureManagment;

namespace PetHome.VolunteerRequests.Application.Features.Read.GetAllAdminVolunteerRequests;
public record GetAllAdminVolunteerRequestsQuery(Guid AdminId, int PageSize, int PageNum):IQuery;
