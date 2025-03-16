using MassTransit;
using NotificationService.Application.Features.GeneralNotification.SendMessageEverywhere;
using PetHome.VolunteerRequests.Contracts.Messaging;

namespace NotificationService.Application.Consumers.VolunteerRequests;

public class SetVolunteerRequestRejectedConsumer(SendMessageEverywhereUseCase useCase)
    : IConsumer<SetVolunteerRequestRejectedEvent>
{
    public async Task Consume(ConsumeContext<SetVolunteerRequestRejectedEvent> context)
    {
        var command = context.Message;
        SendMessageEverywhereCommand re = new SendMessageEverywhereCommand(
             command.UserId,
             $"К сожалению, Ваша заявка {command.VolunteerRequestId} была отклонена. Причина: {command.RejectedComment}",
             "Заявка отклонена");
        await useCase.Execute(re, CancellationToken.None);

        return;
    }
}