using Microsoft.AspNetCore.Mvc;
using PetHome.Core.Controllers;
using PetHome.Core.ValueObjects.User;
using PetHome.Core.ValueObjects.VolunteerRequest;
using PetHome.VolunteerRequests.API.Requests;
using PetHome.VolunteerRequests.Application.Features.Read.GetAllAdminVolunteerRequests;
using PetHome.VolunteerRequests.Application.Features.Read.GetAllSubmittedVolunteerRequests;
using PetHome.VolunteerRequests.Application.Features.Read.GetAllUserVolunteerRequests;
using PetHome.VolunteerRequests.Application.Features.Write.CreateVolunteerRequest;
using PetHome.VolunteerRequests.Application.Features.Write.SetVolunteerRequestApproved;
using PetHome.VolunteerRequests.Application.Features.Write.SetVolunteerRequestOnReview;
using PetHome.VolunteerRequests.Application.Features.Write.SetVolunteerRequestRejected;
using PetHome.VolunteerRequests.Application.Features.Write.SetVolunteerRequestRevisionRequired;
using PetHome.VolunteerRequests.Application.Features.Write.SetVolunteerRequestSubmitted;

namespace PetHome.VolunteerRequests.API;
public class VolunteerRequestsController : ParentController
{
    [HttpGet("volunteer-requests/paged/admin={adminId:guid}")]
    public async Task<IActionResult> GetPagedDiscussionsByRelationId(
       [FromServices] GetAllAdminVolunteerRequestsUseCase useCase,
       [FromRoute] Guid adminId,
       [FromQuery] GetAllAdminVolunteerRequestsRequest request,
       CancellationToken ct)
    {
        var result = await useCase.Execute(request.ToQuery(adminId), ct);
        if (result.IsFailure)
            return BadRequest(result.Error);

        return Ok(result.Value);
    }


    [HttpGet("volunteer-requests/paged/submitted")]
    public async Task<IActionResult> GetPagedAllSubmittedVolunteerRequests(
       [FromServices] GetAllSubmittedVolunteerRequestsUseCase useCase,
       [FromQuery] GetAllSubmittedVolunteerRequestsRequest request,
       CancellationToken ct)
    {
        var result = await useCase.Execute(request, ct);
        if (result.IsFailure)
            return BadRequest(result.Error);

        return Ok(result.Value);
    }


    [HttpGet("volunteer-requests/paged/user/{userId:guid}")]
    public async Task<IActionResult> GetPagedAllSubmittedVolunteerRequests(
       [FromServices] GetAllUserVolunteerRequestsUseCase useCase,
       [FromRoute] Guid userId,
       [FromQuery] GetAllUserVolunteerRequestsRequest request,
       CancellationToken ct)
    {
        var result = await useCase.Execute(request.ToQuery(userId), ct);
        if (result.IsFailure)
            return BadRequest(result.Error);

        return Ok(result.Value);
    }


    [HttpPost("volunteer-requests")]
    public async Task<IActionResult> CreateVolunteerRequest(
       [FromServices] CreateVolunteerRequestUseCase useCase,
       [FromQuery] CreateVolunteerRequestRequest request,
       CancellationToken ct)
    {
        var result = await useCase.Execute(request, ct);
        if (result.IsFailure)
            return BadRequest(result.Error);

        return Ok();
    }


    [HttpPost("volunteer-requests/{volunteerRequestId}/approved/admin={adminId:guid}")]
    public async Task<IActionResult> SetVolunteerRequestApproved(
       [FromServices] SetVolunteerRequestApprovedUseCase useCase,
       [FromRoute] Guid volunteerRequestId,
       [FromRoute] Guid adminId,
       [FromBody] SetVolunteerRequestApprovedRequest request,
       CancellationToken ct)
    { 
        var result = await useCase.Execute(request.ToCommand(volunteerRequestId,adminId), ct);
        if (result.IsFailure)
            return BadRequest(result.Error);

        return Ok();
    }


    [HttpPost("volunteer-requests/{volunteerRequestId}/on-review/discussion={discussionId:guid}/user={userId:guid}")]
    public async Task<IActionResult> SetVolunteerRequestOnReview(
       [FromServices] SetVolunteerRequestOnReviewUseCase useCase,
       [FromBody] SetVolunteerRequestOnReviewRequest request,
       [FromRoute] Guid volunteerRequestId,
       [FromRoute] Guid discussionId, 
       [FromRoute] Guid userId, 
       CancellationToken ct)
    { 
        var result = await useCase.Execute(request.ToCommand(volunteerRequestId, discussionId, userId), ct);
        if (result.IsFailure)
            return BadRequest(result.Error);

        return Ok();
    }


    [HttpPost("volunteer-requests/{volunteerRequestId}/rejected/admin={adminId:guid}")]
    public async Task<IActionResult> SetVolunteerRequestRejected(
       [FromServices] SetVolunteerRequestRejectedUseCase useCase,
       [FromRoute] Guid volunteerRequestId,
       [FromRoute] Guid adminId,
       [FromBody] SetVolunteerRequestRejectedRequest request,
       CancellationToken ct)
    {
        var result = await useCase.Execute(request.ToCommand(volunteerRequestId, adminId), ct);
        if (result.IsFailure)
            return BadRequest(result.Error);

        return Ok();
    }


    [HttpPost("volunteer-requests/{volunteerRequestId}/revision-required/admin={adminId:guid}")]
    public async Task<IActionResult> SetVolunteerRequestRevisionRequired(
       [FromServices] SetVolunteerRequestRevisionRequiredUseCase useCase,
       [FromRoute] Guid volunteerRequestId,
       [FromRoute] Guid adminId,
       [FromBody] SetVolunteerRequestRevisionRequiredRequest request,
       CancellationToken ct)
    {
        var result = await useCase.Execute(request.ToCommand(volunteerRequestId, adminId), ct);
        if (result.IsFailure)
            return BadRequest(result.Error);

        return Ok();
    }


    [HttpPost("volunteer-requests/{volunteerRequestId}/submitted/admin={adminId:guid}")]
    public async Task<IActionResult> SetVolunteerRequestSubmitted(
       [FromServices] SetVolunteerRequestSubmittedUseCase useCase,
       [FromRoute] Guid volunteerRequestId,
       [FromRoute] Guid adminId,
       CancellationToken ct)
    {
        SetVolunteerRequestSubmittedRequest request = new SetVolunteerRequestSubmittedRequest(volunteerRequestId, adminId);

        var result = await useCase.Execute(request, ct);
        if (result.IsFailure)
            return BadRequest(result.Error);

        return Ok();
    }
}
