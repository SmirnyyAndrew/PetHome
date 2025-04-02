using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using PetHome.Core.Application.Interfaces.FeatureManagement;
using PetHome.Core.Domain.Models;
using PetHome.Core.Web.Extentions.Collection;
using PetHome.Core.Web.Extentions.ErrorExtentions;
using PetHome.SharedKernel.Responses.ErrorManagement;
using PetManagementService.Application.Database;
using PetManagementService.Application.Database.Dto;

namespace PetManagementService.Application.Features.Read.VolunteerManegment.GetAllVolunteersWithPagination;
public class GetAllVolunteersWithPaginationUseCase
    : IQueryHandler<PagedList<VolunteerDto>, GetAllVolunteersWithPaginationQuery>
{
    private readonly IPetManagementReadDbContext _readDBContext;
    private readonly ILogger<GetAllVolunteersWithPaginationUseCase> _logger;

    public GetAllVolunteersWithPaginationUseCase(
        IPetManagementReadDbContext readDBContext,
        ILogger<GetAllVolunteersWithPaginationUseCase> logger)
    {
        _readDBContext = readDBContext;
        _logger = logger;
    }

    public async Task<Result<PagedList<VolunteerDto>, ErrorList>> Execute(
        GetAllVolunteersWithPaginationQuery query,
        CancellationToken ct)
    {
        if (query.PageNum == 0 || query.PageSize == 0)
            return Errors.Validation("Номер и размер страницы не может быть меньше 1").ToErrorList();

        var pagedVolunteerDtos = await _readDBContext.Volunteers
            .ToPagedList(query.PageNum, query.PageSize, ct);

        _logger.LogInformation("Получено {0} волонётров", pagedVolunteerDtos.Count);

        return pagedVolunteerDtos;
    }
}
