using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using PetHome.Core.Interfaces.FeatureManagment;
using PetHome.Domain.Shared.Error;

namespace PetHome.Volunteers.Application.Features.Read.PetManegment.Pet.GetPetById;
public class GetPetByIdUseCase
    : IQueryHandler<PetDto, GetPetByIdQuery>
{
    private readonly IReadDBContext _readDBContext;
    private readonly ILogger<GetPetByIdUseCase> _logger;

    public GetPetByIdUseCase(
        IReadDBContext readDBContext,
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
