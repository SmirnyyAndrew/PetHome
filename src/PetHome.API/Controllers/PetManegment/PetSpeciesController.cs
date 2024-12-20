using Microsoft.AspNetCore.Mvc;
using PetHome.Application.Features.Read.PetManegment.Species.GetAllSpecies;
using PetHome.Application.Features.Write.PetManegment.CreateSpecies;

namespace PetHome.API.Controllers.PetManegment;

public class PetSpeciesController : ParentController
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

    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromServices] GetAllSpeciesUseCase useCase,
        CancellationToken ct)
    {
        var result = await useCase.Execute(ct);
        return Ok(result);
    }

}
