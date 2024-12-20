using Microsoft.AspNetCore.Mvc;
using PetHome.API.Controllers.PetManegment.Requests;
using PetHome.Application.Features.Read.PetManegment.GetAllBreedDtoBySpeciesId;
using PetHome.Application.Features.Write.PetManegment.CreateBreed;

namespace PetHome.API.Controllers.PetManegment;

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
        var result = await useCase.Execute(id, ct);
        if (result.IsFailure)
            return BadRequest(result.Error);

        return Ok(result.Value);
    }
}
