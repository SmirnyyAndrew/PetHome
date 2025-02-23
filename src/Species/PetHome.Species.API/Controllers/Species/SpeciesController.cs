using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetHome.Core.Controllers;
using PetHome.Species.API.Controllers.Species.Requests;
using PetHome.Species.Application.Features.Read.Species.GetAllSpecies;
using PetHome.Species.Application.Features.Write.CreateSpecies;

namespace PetHome.Species.API.Controllers.Species;

public class SpeciesController : ParentController
{
    [HttpPost]
    public async Task<IActionResult> CreateSpecies(
        [FromBody] CreateSpeciesRequest request,
        [FromServices] CreateSpeciesUseCase useCase,
        CancellationToken ct = default)
    {
        var result = await useCase.Execute(request, ct);
        if (result.IsFailure)
            return BadRequest(result.Error);

        return Ok(result.Value);
    }


    [HttpGet("breeds/paged")]
    public async Task<IActionResult> GetAllSpecies(
        [FromServices] GetAllSpeciesUseCase useCase,
        [FromQuery] GetAllSpeciesRequest request,
        CancellationToken ct)
    {
        var result = await useCase.Execute(request, ct);
        if (result.IsFailure)
            return BadRequest(result.Error);

        return Ok(result.Value);
    }

}
