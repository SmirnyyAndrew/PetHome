using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PetHome.Application.Database.Read;
using PetHome.Application.Models;
using PetHome.Domain.Shared.Error;

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

    public async Task<Result<PagedList<VolunteerDto>, Error>> Execute(
        GetAllVolunteersWithPaginationQuery query,
        CancellationToken ct)
    {
        if (query.PageNum == 0 || query.PageSize == 0)
            return Errors.Validation("Номер и размер страницы не может быть меньше 1");

        var volunteerDtos = await _readDBContext.Volunteers
            .Skip((query.PageNum - 1) * query.PageSize)
            .Take(query.PageSize)
            .ToListAsync();

        _logger.LogInformation("Получено {0} волонётров", volunteerDtos.Count);

        PagedList<VolunteerDto> pagedVolunteersDto = new PagedList<VolunteerDto>()
        {
            Items = volunteerDtos,
            PageNumber = query.PageNum,
            PageSize = query.PageSize
        };

        return pagedVolunteersDto;
    }
}
