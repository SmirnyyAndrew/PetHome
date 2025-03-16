using MassTransit;
using NotificationService.Application.Features.GeneralNotification.SendMessageEverywhere;
using PetHome.VolunteerRequests.Contracts.Messaging;

namespace NotificationService.Application.Consumers.VolunteerRequests;

public class SetVolunteerRequestSubmittedConsumer(SendMessageEverywhereUseCase useCase)
    : IConsumer<SetVolunteerRequestSubmittedEvent>
{
    public async Task Consume(ConsumeContext<SetVolunteerRequestSubmittedEvent> context)
    {
        var command = context.Message;
        SendMessageEverywhereCommand re = new SendMessageEverywhereCommand(
             command.UserId,
             $"Ваша заявка была взата на рассмотрение",
             "Рассмотрение заявки");
        await useCase.Execute(re, CancellationToken.None);

        return;
    }
}