using Microsoft.AspNetCore.Mvc;
using PetHome.Core.API.Controllers;
using PetManagementService.API.Controllers.Breed.Requests;
using PetManagementService.Application.Features.Read.Breeds.GetAllBreedDtoBySpeciesId;
using PetManagementService.Application.Features.Write.SpeciesManagement.CreateBreed;

namespace PetManagementService.API.Controllers.Breed;

public class BreedController : ParentController
{
    [HttpPost]
    public async Task<IActionResult> CreateBreed(
        [FromBody] CreateBreedRequest request,
        [FromServices] CreateBreedUseCase useCase,
        CancellationToken ct = default)
    {
        var createBreedResult = await useCase.Execute(
            request,
            ct);
        if (createBreedResult.IsFailure)
            return BadRequest(createBreedResult.Error);


        return Ok(createBreedResult.Value);
    }

    [HttpGet("species/{speciesId:guid}")]
    public async Task<IActionResult> GetAllBreedsBySpeciesId(
        [FromRoute] Guid speciesId,
        [FromQuery] GetAllBreedDtoBySpeciesIdRequest request,
        [FromServices] GetAllBreedDtosBySpeciesIdUseCase useCase,
        CancellationToken ct)
    { 
        var result = await useCase.Execute(request.ToQuery(speciesId), ct);
        if (result.IsFailure)
            return BadRequest(result.Error);

        return Ok(result.Value);
    }
}
