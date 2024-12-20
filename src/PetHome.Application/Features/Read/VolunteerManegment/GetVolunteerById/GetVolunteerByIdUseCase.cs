using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PetHome.Application.Database.Read;
using PetHome.Domain.Shared.Error;

namespace PetHome.Application.Features.Read.VolunteerManegment.GetVolunteerById;
public class GetVolunteerByIdUseCase
{
    private readonly IReadDBContext _readDBContext;
    private readonly ILogger<GetVolunteerByIdUseCase> _logger;

    public GetVolunteerByIdUseCase(
        IReadDBContext readDBContext,
        ILogger<GetVolunteerByIdUseCase> logger)
    {
        _readDBContext = readDBContext;
        _logger = logger;
    }

    public async Task<Result<VolunteerDto, Error>> Execute(
        Guid id,
        CancellationToken ct)
    {
        VolunteerDto? volunteer = await _readDBContext.Volunteers
            .Where(x => x.Id == id)
            .FirstOrDefaultAsync();
        if (volunteer == null)
        {
            _logger.LogError("Волонтёр с id = {0} не найден", id);
            return Errors.NotFound($"Волонтёр с id = {id}");
        }
        return volunteer;
    }
}
