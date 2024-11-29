using Microsoft.AspNetCore.Mvc;
using PetHome.Application.Volunteers.CreateVolunteer;

namespace PetHome.API.Controllers;

[ApiController]
[Route("[controller]")]
public class VolunteerController : ControllerBase
{
    [HttpPut]
    public async Task<IActionResult> Create(
        [FromServices] CreateVolunteerUseCase useCase,
        [FromBody] CreateVolunteerRequest request,
        CancellationToken ct = default)
    {
        var result = useCase.Execute(request, ct);

        if (result.IsFaulted)
            return BadRequest("Не удалось создать волонтёра");
         
        return Ok($"id = {result}");
    }
}
