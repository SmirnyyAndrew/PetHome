using DiscussionService.Contracts.Messaging;
using MassTransit;
using NotificationService.Application.Features.GeneralNotification.SendMessageEverywhere;

namespace NotificationService.Application.Consumers.Discussions;

public class CloseDiscussionConsumer(SendMessageEverywhereUseCase useCase)
    : IConsumer<ClosedDiscussionEvent>
{
    public async Task Consume(ConsumeContext<ClosedDiscussionEvent> context)
    {
        var command = context.Message;
        command.UsersIds.Select(async id =>
        {
            SendMessageEverywhereCommand re = new SendMessageEverywhereCommand(
                 id,
                 $"Дискуссия {command.DiscussionId} закрыта",
                 "Закрытие дискуссии");
            await useCase.Execute(re, CancellationToken.None);
        });

        return;
    }
}