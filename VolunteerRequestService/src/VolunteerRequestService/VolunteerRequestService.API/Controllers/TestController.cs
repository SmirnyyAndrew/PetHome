using Microsoft.AspNetCore.Mvc;

namespace VolunteerRequestService.API.Controllers;
[ApiController]
[Route("api/[controller]")]
public class TestController : ControllerBase
{
    [HttpGet("test")]
    public async Task<IActionResult> GetPagedDiscussionsByRelationId(CancellationToken ct)
    { 
        return Ok("Evrything is ok!");
    }
}
