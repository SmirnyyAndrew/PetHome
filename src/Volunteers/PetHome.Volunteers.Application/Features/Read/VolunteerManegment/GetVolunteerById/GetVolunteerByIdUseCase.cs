using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PetHome.Core.Extentions.ErrorExtentions;
using PetHome.Core.Interfaces.FeatureManagment;
using PetHome.Core.Response.ErrorManagment;
using PetHome.Core.Response.Validation.Validator;
using PetHome.Volunteers.Application.Database;
using PetHome.Volunteers.Application.Database.Dto;

namespace PetHome.Volunteers.Application.Features.Read.VolunteerManegment.GetVolunteerById;
public class GetVolunteerByIdUseCase
    : IQueryHandler<VolunteerDto, GetVolunteerByIdQuery>
{
    private readonly IVolunteerReadDbContext _readDBContext;
    private readonly ILogger<GetVolunteerByIdUseCase> _logger;

    public GetVolunteerByIdUseCase(
        IVolunteerReadDbContext readDBContext,
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
            return Errors.NotFound($"Волонтёр с id = {query.VolunteerId}").ToErrorList();
        }
        return volunteer;
    }
}
