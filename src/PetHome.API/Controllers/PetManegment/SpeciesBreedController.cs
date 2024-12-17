using Microsoft.AspNetCore.Mvc;
using PetHome.Application.Features.Volunteers.PetManegment.CreateSpecies;

namespace PetHome.API.Controllers.PetManegment;

public class SpeciesBreedController : ParentController
{
    [HttpPost("species")]
    public async Task<IActionResult> CreateSpecies(
        [FromBody] string speciesName,
        [FromServices] CreateSpeciesUseCase createSpeciesUseCase,
        CancellationToken ct = default)
    {
        var result = await createSpeciesUseCase.Execute(speciesName, ct);
        if (result.IsFailure)
            return BadRequest(ResponseEnvelope.Error(result.Error));

        return Ok(ResponseEnvelope.Ok(result.Value));
    }

    [HttpPost("breeds")]
    public async Task<IActionResult> CreateBreed(
        [FromBody] CreateBreedRequst createBreedRequst,
        [FromServices] CreateBreedUseCase createBreedUseCase,
        CancellationToken ct = default)
    {
        var createBreedResult = await createBreedUseCase.Execute(
            createBreedRequst,
            ct);
        if (createBreedResult.IsFailure)
            return BadRequest(ResponseEnvelope.Error(createBreedResult.Error));


        return Ok(ResponseEnvelope.Ok(createBreedResult.Value));
    }
}
