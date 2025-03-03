using Microsoft.AspNetCore.Mvc;
using NotificationService.Application.Features.Email.SendMessage;

namespace NotificationService.Web.Controllers;

public class EmailController : ParentController
{
    [HttpPost("email/message")]
    public async Task<IActionResult> UpdateUserNotificationSettings( 
        [FromBody] SendMessageCommand request,
        [FromServices] SendMessageUseCase useCase,
        CancellationToken ct = default)
    {
        await useCase.Execute(request, ct);
        return Ok();
    }
}
