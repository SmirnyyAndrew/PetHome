using PetHome.Core.Application.Interfaces.FeatureManagement;
namespace PetHome.VolunteerRequests.Application.Features.Read.GetAllSubmittedVolunteerRequests;
public record GetAllSubmittedVolunteerRequestsQuery(int PageSize, int PageNum) : IQuery; 
