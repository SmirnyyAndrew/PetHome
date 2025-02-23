using Microsoft.AspNetCore.Mvc;
using PetHome.Core.Controllers;
using PetHome.Volunteers.Application.Features.Write.FilesService.GetFilesDataByIds;

namespace PetHome.Volunteers.API.Controllers.Media;
public class FilesServiceController : ParentController
{ 
    [HttpPost]
    public async Task<IActionResult> GetFilesDataByIds(
        [FromServices] GetFilesDataByIdsUseCase useCase,
        [FromBody] GetFilesDataByIdsCommand request,
        CancellationToken ct = default)
    { 
        var result = await useCase.Execute(request, ct);
        if (result.IsFailure)
            return BadRequest(result.Error);

        return Ok(result.Value);
    } 
}
