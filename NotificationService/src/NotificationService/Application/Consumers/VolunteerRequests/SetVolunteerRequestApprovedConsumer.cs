using MassTransit;
using NotificationService.Application.Features.GeneralNotification.SendMessageEverywhere;
using PetHome.VolunteerRequests.Contracts.Messaging;

namespace NotificationService.Application.Consumers.VolunteerRequests;

public class SetVolunteerRequestApprovedConsumer(SendMessageEverywhereUseCase useCase)
    : IConsumer<SetVolunteerRequestApprovedEvent>
{
    public async Task Consume(ConsumeContext<SetVolunteerRequestApprovedEvent> context)
    {
        var command = context.Message;
        SendMessageEverywhereCommand re = new SendMessageEverywhereCommand(
             command.UserId,
             $"Поздравляем! Ваша заявка {command.VolunteerRequestId} одобрена",
             "Заявка одобрена");
        await useCase.Execute(re, CancellationToken.None);
        return;
    }
}