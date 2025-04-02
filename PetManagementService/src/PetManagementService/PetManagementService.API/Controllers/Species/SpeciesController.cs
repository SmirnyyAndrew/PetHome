using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetHome.Core.API.Controllers;
using PetManagementService.API.Controllers.Species.Requests;
using PetManagementService.Application.Features.Read.Species.GetAllSpecies;
using PetManagementService.Application.Features.Write.SpeciesManagement.CreateSpecies;

namespace PetManagementService.API.Controllers.Species;

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
