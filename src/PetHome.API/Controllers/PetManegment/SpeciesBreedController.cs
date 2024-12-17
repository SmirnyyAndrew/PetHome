using Microsoft.AspNetCore.Mvc;
using PetHome.API.Controllers.PetManegment.Requests;
using PetHome.Application.Features.Volunteers.PetManegment.CreateBreed;
using PetHome.Application.Features.Volunteers.PetManegment.CreateSpecies;

namespace PetHome.API.Controllers.PetManegment;

public class SpeciesBreedController : ParentController
{
    [HttpPost("species")]
    public async Task<IActionResult> CreateSpecies(
        [FromBody] CreateSpeciesCommand createSpeciesCommand,
        [FromServices] CreateSpeciesUseCase createSpeciesUseCase,
        CancellationToken ct = default)
    {
        var result = await createSpeciesUseCase.Execute(createSpeciesCommand, ct);
        if (result.IsFailure)
            return BadRequest(result.Error);

        return Ok(result.Value);
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
            return BadRequest(createBreedResult.Error);


        return Ok(createBreedResult.Value);
    }
}
