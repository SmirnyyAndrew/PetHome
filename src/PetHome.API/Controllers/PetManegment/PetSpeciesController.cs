using Microsoft.AspNetCore.Mvc;
using PetHome.Application.Features.Write.PetManegment.CreateSpecies;

namespace PetHome.API.Controllers.PetManegment;

public class PetSpeciesController : ParentController
{
    [HttpPost("species")]
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
}
