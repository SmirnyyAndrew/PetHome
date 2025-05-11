using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PetHome.Core.Application.Interfaces.FeatureManagement;
using PetHome.Core.Web.Extentions.ErrorExtentions;
using PetHome.SharedKernel.Responses.ErrorManagement;
using PetManagementService.Application.Database;
using PetManagementService.Application.Database.Dto;

namespace PetManagementService.Application.Features.Read.VolunteerManegment.GetVolunteerById;
public class GetVolunteerByIdUseCase
    : IQueryHandler<VolunteerDto, GetVolunteerByIdQuery>
{
    private readonly IPetManagementReadDbContext _readDBContext;
    private readonly ILogger<GetVolunteerByIdUseCase> _logger;

    public GetVolunteerByIdUseCase(
        IPetManagementReadDbContext readDBContext,
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
