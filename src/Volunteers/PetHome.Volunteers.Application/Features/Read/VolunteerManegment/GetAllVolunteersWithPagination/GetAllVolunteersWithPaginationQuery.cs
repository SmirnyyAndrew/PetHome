using PetHome.Core.Interfaces.FeatureManagment;

namespace PetHome.Volunteers.Application.Features.Read.VolunteerManegment.GetAllVolunteersWithPagination;
public record GetAllVolunteersWithPaginationQuery(int PageSize, int PageNum) : IQuery;
