using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PetHome.Core.Application.Interfaces.FeatureManagement;
using PetHome.Core.Domain.Models;
using PetHome.Core.Web.Extentions.Collection;
using PetHome.Core.Web.Extentions.ErrorExtentions;
using PetHome.SharedKernel.Responses.ErrorManagement;
using PetManagementService.Application.Database;
using PetManagementService.Application.Database.Dto;

namespace PetManagementService.Application.Features.Read.Breeds.GetAllBreedDtoBySpeciesId;
public class GetAllBreedDtosBySpeciesIdUseCase
    : IQueryHandler<PagedList<BreedDto>, GetAllBreedDtosBySpeciesIdQuery>
{
    private readonly IPetManagementReadDbContext _readDBContext;
    private readonly ILogger<GetAllBreedDtosBySpeciesIdUseCase> _logger;

    public GetAllBreedDtosBySpeciesIdUseCase(
        IPetManagementReadDbContext readDBContext,
        ILogger<GetAllBreedDtosBySpeciesIdUseCase> logger)
    {
        _readDBContext = readDBContext;
        _logger = logger;
    }

    public async Task<Result<PagedList<BreedDto>, ErrorList>> Execute(
        GetAllBreedDtosBySpeciesIdQuery query,
        CancellationToken ct)
    {
        SpeciesDto? getSpeciesByIdResult = await _readDBContext.Species
            .Include(b => b.Breeds)
            .Where(s => s.Id == query.SpeciesId)
            .FirstOrDefaultAsync();
        if (getSpeciesByIdResult == null)
        {
            _logger.LogError("Вид животного с id - {0} не найден", query.SpeciesId);
            return Errors.NotFound($"вид животного с id - {query.SpeciesId}").ToErrorList();
        }

        var pagedBreedDtos = getSpeciesByIdResult.Breeds
            .ToPagedList(query.PageNum, query.PageSize);
        return pagedBreedDtos; 
    }
}
