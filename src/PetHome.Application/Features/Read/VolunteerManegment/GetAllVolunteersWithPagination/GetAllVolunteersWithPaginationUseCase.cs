using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PetHome.Application.Database.Read;
using PetHome.Application.Models;

namespace PetHome.Application.Features.Read.VolunteerManegment.GetAllVolunteersWithPagination;
public class GetAllVolunteersWithPaginationUseCase
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

    public async Task<Result<PagedList<VolunteerDto>>> Execute(
        GetAllVolunteersWithPaginationQuery query,
        CancellationToken ct)
    {
        var volunteerDtos = await _readDBContext.Volunteers
            .Skip((query.PageNum - 1) * query.PageSize)
            .Take(query.PageSize)
            .ToListAsync();

        _logger.LogInformation("Получено {0} волонётров", volunteerDtos.Count);

        PagedList<VolunteerDto> record = new PagedList<VolunteerDto>()
        {
            Items = volunteerDtos,
            PageNumber = query.PageNum,
            PageSize = query.PageSize
        };

        return record;
    }
}
