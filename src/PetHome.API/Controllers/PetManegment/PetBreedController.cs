using Microsoft.AspNetCore.Mvc;
using PetHome.API.Controllers.PetManegment.Requests;
using PetHome.Application.Features.Write.PetManegment.CreateBreed;

namespace PetHome.API.Controllers.PetManegment;

public class PetBreedController : ParentController
{
    [HttpPost("breeds")]
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
}
