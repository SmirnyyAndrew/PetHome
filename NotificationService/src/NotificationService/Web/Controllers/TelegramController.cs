using Microsoft.AspNetCore.Mvc;
using NotificationService.Application.Features.Telegram.SendMessage;

namespace NotificationService.Web.Controllers;

public class TelegramController : ParentController
{
    [HttpPost("telegram/message")]
    public async Task<IActionResult> SendMessage(
        [FromServices] SendTelegramMessageUseCase useCase,
        [FromBody] SendTelegramMessageCommand command,
        CancellationToken ct = default)
    {
        await useCase.Execute(command, ct);
        return Ok();
    }
}
