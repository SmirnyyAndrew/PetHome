using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using PetHome.Core.Extentions.ErrorExtentions;
using PetHome.Core.Interfaces.FeatureManagment;
using PetHome.Core.Response.ErrorManagment;
using PetHome.Core.Response.Validation.Validator;
using PetHome.Volunteers.Application.Database;
using PetHome.Volunteers.Application.Database.Dto;

namespace PetHome.Volunteers.Application.Features.Read.PetManegment.Pet.GetPetById;
public class GetPetByIdUseCase
    : IQueryHandler<PetDto, GetPetByIdQuery>
{
    private readonly IVolunteerReadDbContext _readDBContext;
    private readonly ILogger<GetPetByIdUseCase> _logger;

    public GetPetByIdUseCase(
        IVolunteerReadDbContext readDBContext,
        ILogger<GetPetByIdUseCase> logger)
    {
        _readDBContext = readDBContext;
        _logger = logger;
    }

    public async Task<Result<PetDto, ErrorList>> Execute(
        GetPetByIdQuery query,
        CancellationToken ct)
    {
        var petDto = _readDBContext.Pets.FirstOrDefault(p => p.Id == query.PetId);
        if (petDto == null)
        {
            _logger.LogError("Питомец с id = {0} не существует", query.PetId);
            return Errors.NotFound($"Питомец с id = {query.PetId}").ToErrorList();
        }

        return petDto;
    }
}
