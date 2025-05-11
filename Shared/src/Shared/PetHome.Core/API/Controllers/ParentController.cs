using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using PetHome.SharedKernel.Responses.ErrorManagement;
using PetHome.SharedKernel.Responses.ErrorManagement.Envelopes;

namespace PetHome.Core.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ParentController : ControllerBase
{
    public override OkObjectResult Ok([ActionResultObjectValue] object? value)
    {
        return base.Ok(ResponseEnvelope.Ok(value));
    }

    public override BadRequestObjectResult BadRequest([ActionResultObjectValue] object? error)
    {
        return base.BadRequest(ResponseEnvelope.Error(error as ErrorList));
    }
}
