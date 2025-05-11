using PetManagementService.Application.Database;
using PetManagementService.Application.Database.Dto;

namespace PetManagementService.Application.Features.GraphQL.PetAggregate;
public class PetGraphQLService(IPetManagementReadDbContext readDbContext)
{
    public IQueryable<PetDto> GetAll() => readDbContext.Pets;
}
