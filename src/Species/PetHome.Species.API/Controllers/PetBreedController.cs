using Microsoft.AspNetCore.Mvc;
using PetHome.Core.Controllers;
using PetHome.Species.API.Controllers.Requests;
using PetHome.Species.Application.Features.Read.Breeds.GetAllBreedDtoBySpeciesId;
using PetHome.Species.Application.Features.Write.CreateBreed;

namespace PetHome.Species.API.Controllers;

public class PetBreedController : ParentController
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

    [HttpGet("species-{id:guid}")]
    public async Task<IActionResult> GetAllBreedsBySpeciesId(
        [FromRoute] Guid id,
        [FromServices] GetAllBreedDtoBySpeciesIdUseCase useCase,
        CancellationToken ct)
    {
        GetAllBreedDtoBySpeciesIdRequest request = new GetAllBreedDtoBySpeciesIdRequest(id);
        var result = await useCase.Execute(request, ct);
        if (result.IsFailure)
            return BadRequest(result.Error);

        return Ok(result.Value);
    }
}
