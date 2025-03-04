using Microsoft.AspNetCore.Mvc;
using NotificationService.Application.Features.Email.SendMessage;

namespace NotificationService.Web.Controllers;

public class EmailController : ParentController
{
    [HttpPost("email/message")]
    public async Task<IActionResult> UpdateUserNotificationSettings( 
        [FromBody] SendEmailMessageCommand request,
        [FromServices] SendEmailMessageUseCase useCase,
        CancellationToken ct = default)
    {
        await useCase.Execute(request, ct);
        return Ok();
    }
}
