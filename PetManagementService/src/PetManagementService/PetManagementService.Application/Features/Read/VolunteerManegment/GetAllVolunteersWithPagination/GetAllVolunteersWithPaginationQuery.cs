using PetHome.Core.Interfaces.FeatureManagment;

namespace PetManagementService.Application.Features.Read.VolunteerManegment.GetAllVolunteersWithPagination;
public record GetAllVolunteersWithPaginationQuery(int PageSize, int PageNum) : IQuery;
