using PetHome.Application.Interfaces.FeatureManagment;

namespace PetHome.Application.Features.Read.VolunteerManegment.GetAllVolunteersWithPagination;
public record GetAllVolunteersWithPaginationQuery(int PageSize, int PageNum) : IQuery;
