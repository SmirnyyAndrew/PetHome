using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using PetHome.Application.Database.Read;

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

    public async Task<Result<IReadOnlyList<VolunteerDto>>> Execute(
        CancellationToken ct)
    {
        var volunteerDtos = _readDBContext.Volunteers.ToList();
        _logger.LogInformation("Получено {0} волонётров", volunteerDtos.Count);
        return volunteerDtos;
    }
}
