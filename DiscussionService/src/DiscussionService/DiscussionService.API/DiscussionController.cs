using DiscussionService.API.Requests;
using DiscussionService.Application.Features.Read.GetAllDiscussionByRelationId;
using DiscussionService.Application.Features.Write.CloseDiscussion;
using DiscussionService.Application.Features.Write.CreateDiscussionUsingContract;
using DiscussionService.Application.Features.Write.EditMessageInDiscussion;
using DiscussionService.Application.Features.Write.OpenDiscussion;
using DiscussionService.Application.Features.Write.RemoveMessageInDiscussion;
using DiscussionService.Application.Features.Write.SendMessageInDiscussion;
using Microsoft.AspNetCore.Mvc;
using PetHome.Core.Controllers;

namespace DiscussionService.API;
public class DiscussionController : ParentController
{
    [HttpGet("discussions/paged/{relationId:guid}")]
    public async Task<IActionResult> GetPagedDiscussionsByRelationId(
       [FromServices] GetAllDiscussionByRelationIdUseCase useCase,
       [FromRoute] Guid relationId,
       [FromQuery] GetAllDiscussionByRelationIdRequest request,
       CancellationToken ct)
    {
        var result = await useCase.Execute(request.ToCommand(relationId), ct);
        if (result.IsFailure)
            return BadRequest(result.Error);

        return Ok(result.Value);
    }


    [HttpPost("close-discussion/{discussionId:guid}")]
    public async Task<IActionResult> CloseDiscussion(
       [FromServices] CloseDiscussionUseCase useCase,
       [FromRoute] Guid discussionId,
       CancellationToken ct)
    {
        CloseDiscussionRequest request = new CloseDiscussionRequest(discussionId);
        var result = await useCase.Execute(request, ct);
        if (result.IsFailure)
            return BadRequest(result.Error);

        return Ok();
    }


    [HttpPost("discussion/{relationId:guid}")]
    public async Task<IActionResult> CreateDiscussion(
       [FromServices] CreateDiscussionUseCase useCase,
       [FromRoute] Guid relationId,
       [FromBody] CreateDiscussionRequest request,
       CancellationToken ct)
    {
        var result = await useCase.Execute(request.ToCommand(relationId), ct);
        if (result.IsFailure)
            return BadRequest(result.Error);

        return Ok(result.Value);
    }


    [HttpPost("edit-message-discussion/{discussionId:guid}")]
    public async Task<IActionResult> EditMessageInDiscussion(
       [FromServices] EditMessageInDiscussionUseCase useCase,
       [FromRoute] Guid discussionId,
       [FromBody] EditMessageInDiscussionRequest request,
       CancellationToken ct)
    {
        var result = await useCase.Execute(request.ToCommand(discussionId), ct);
        if (result.IsFailure)
            return BadRequest(result.Error);

        return Ok();
    }


    [HttpPost("open-discussion/{discussionId:guid}")]
    public async Task<IActionResult> OpenDiscussion(
       [FromServices] OpenDiscussionUseCase useCase,
       [FromRoute] Guid discussionId,
       CancellationToken ct)
    {
        OpenDiscussionRequest request = new OpenDiscussionRequest(discussionId);
        var result = await useCase.Execute(request, ct);
        if (result.IsFailure)
            return BadRequest(result.Error);

        return Ok();
    }


    [HttpDelete("discussion/messages/{messageId:guid}")]
    public async Task<IActionResult> RemoveMessageInDiscussion(
       [FromServices] RemoveMessageInDiscussionUseCase useCase,
       [FromRoute] Guid messageId,
       [FromBody] RemoveMessageInDiscussionRequest request,
       CancellationToken ct)
    {
        var result = await useCase.Execute(request.ToCommand(messageId), ct);
        if (result.IsFailure)
            return BadRequest(result.Error);

        return Ok();
    }


    [HttpPost("discussion/{discussionId:guid}/messages")]
    public async Task<IActionResult> SendMessageInDiscussion(
       [FromServices] SendMessageInDiscussionUseCase useCase,
       [FromRoute] Guid discussionId,
       [FromBody] SendMessageInDiscussionRequest request,
       CancellationToken ct)
    {
        var result = await useCase.Execute(request.ToCommand(discussionId), ct);
        if (result.IsFailure)
            return BadRequest(result.Error);

        return Ok();
    }
}
