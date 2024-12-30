using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using PetHome.Core.Extentions.Collection;
using PetHome.Core.Extentions.ErrorExtentions;
using PetHome.Core.Interfaces.FeatureManagment;
using PetHome.Core.Models;
using PetHome.Core.Response.ErrorManagment;
using PetHome.Core.Response.Validation.Validator;
using PetHome.Volunteers.Application.Database;
using PetHome.Volunteers.Application.Database.Dto;

namespace PetHome.Volunteers.Application.Features.Read.VolunteerManegment.GetAllVolunteersWithPagination;
public class GetAllVolunteersWithPaginationUseCase
    : IQueryHandler<PagedList<VolunteerDto>, GetAllVolunteersWithPaginationQuery>
{
    private readonly IVolunteerReadDbContext _readDBContext;
    private readonly ILogger<GetAllVolunteersWithPaginationUseCase> _logger;

    public GetAllVolunteersWithPaginationUseCase(
        IVolunteerReadDbContext readDBContext,
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
