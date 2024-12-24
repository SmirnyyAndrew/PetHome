using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PetHome.Application.Database.Read;
using PetHome.Application.Interfaces.FeatureManagment;
using PetHome.Application.Validator;
using PetHome.Domain.Shared.Error;

namespace PetHome.Application.Features.Read.VolunteerManegment.GetVolunteerById;
public class GetVolunteerByIdUseCase
    : IQueryHandler<VolunteerDto, GetVolunteerByIdQuery>
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

    public async Task<Result<VolunteerDto, ErrorList>> Execute(
        GetVolunteerByIdQuery query,
        CancellationToken ct)
    {
        VolunteerDto? volunteer = await _readDBContext.Volunteers
            .Where(x => x.Id == query.VolunteerId)
            .FirstOrDefaultAsync();
        if (volunteer == null)
        {
            _logger.LogError("Волонтёр с id = {0} не найден", query.VolunteerId);
            return (ErrorList)Errors.NotFound($"Волонтёр с id = {query.VolunteerId}");
        }
        return volunteer;
    }
}
