using Microsoft.AspNetCore.Mvc;
using PetHome.Core.Controllers;
using PetManagementService.Application.Features.Write.FilesService.GetFilesDataByIds;

namespace PetManagementService.API.Controllers.Media;
public class FilesController : ParentController
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
