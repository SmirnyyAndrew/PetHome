using PetHome.Core.Interfaces.FeatureManagment;

namespace PetHome.VolunteerRequests.Application.Features.Read.GetAllSubmittedVolunteerRequests;
public record GetAllSubmittedVolunteerRequestsQuery(int PageSize, int PageNum) : IQuery; 
