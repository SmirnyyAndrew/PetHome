using MassTransit;
using NotificationService.Application.Features.GeneralNotification.SendMessageEverywhere;
using DiscussionService.Contracts.Messaging;

namespace NotificationService.Application.Consumers.Discussions;

public class OpenDiscussionConsumer(SendMessageEverywhereUseCase useCase)
    : IConsumer<OpenedDiscussionEvent>
{
    public async Task Consume(ConsumeContext<OpenedDiscussionEvent> context)
    {
        var command = context.Message;
        command.UsersIds.Select(async id =>
        {
            SendMessageEverywhereCommand re = new SendMessageEverywhereCommand(
                 id,
                 $"Дискуссия {command.DiscussionId} открыта",
                 "Открытие дискуссии");
            await useCase.Execute(re, CancellationToken.None);
        });

        return;
    }
}
