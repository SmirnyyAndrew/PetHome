using MassTransit;
using NotificationService.Application.Features.GeneralNotification.SendMessageEverywhere;
using DiscussionService.Contracts.Messaging;

namespace NotificationService.Application.Consumers.Discussions;

public class CreateDiscussionConsumer(SendMessageEverywhereUseCase useCase)
    : IConsumer<CreatedDiscussionEvent>
{
    public async Task Consume(ConsumeContext<CreatedDiscussionEvent> context)
    {
        var command = context.Message;
        command.UsersIds.Select(async id =>
        {
            SendMessageEverywhereCommand re = new SendMessageEverywhereCommand(
                 id,
                 $"Дискуссия {command.DiscussionId} открыта",
                 "Начало дискуссии");
            await useCase.Execute(re, CancellationToken.None);
        });

        return;
    }
}