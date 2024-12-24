using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PetHome.Application.Database.Read;
using PetHome.Application.Extentions;
using PetHome.Application.Interfaces.FeatureManagment;
using PetHome.Application.Models;
using PetHome.Application.Validator;
using PetHome.Domain.Shared.Error;

namespace PetHome.Application.Features.Read.VolunteerManegment.GetAllVolunteersWithPagination;
public class GetAllVolunteersWithPaginationUseCase
    : IQueryHandler<PagedList<VolunteerDto>, GetAllVolunteersWithPaginationQuery>
{
    private readonly IReadDBContext _readDBContext;
    private readonly ILogger<GetAllVolunteersWithPaginationUseCase> _logger;

    public GetAllVolunteersWithPaginationUseCase(
        IReadDBContext readDBContext,
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
