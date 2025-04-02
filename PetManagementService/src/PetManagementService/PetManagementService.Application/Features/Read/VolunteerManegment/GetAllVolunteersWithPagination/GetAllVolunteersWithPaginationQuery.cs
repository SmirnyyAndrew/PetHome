using PetHome.Core.Application.Interfaces.FeatureManagement;

namespace PetManagementService.Application.Features.Read.VolunteerManegment.GetAllVolunteersWithPagination;
public record GetAllVolunteersWithPaginationQuery(int PageSize, int PageNum) : IQuery;
