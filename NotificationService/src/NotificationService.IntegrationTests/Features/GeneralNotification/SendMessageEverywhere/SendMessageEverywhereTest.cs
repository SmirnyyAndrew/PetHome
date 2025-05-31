using Microsoft.Extensions.DependencyInjection;
using NotificationService.Application.Features.Email.SendMessage;
using NotificationService.Application.Features.GeneralNotification.SendMessageEverywhere;
using NotificationService.Domain;
using NotificationService.IntegrationTests.IntegrationFactories;
using PetHome.Core.Application.Interfaces.FeatureManagement;
using Xunit;

namespace NotificationService.IntegrationTests.Features.Email.SendMessageTest;

public class SendMessageEverywhereTest : NotificationFactory
{
    private readonly ICommandHandler<SendMessageEverywhereCommand> _sut;

    public SendMessageEverywhereTest(IntegrationTestFactory factory) : base(factory)
    {
        _sut = _scope.ServiceProvider.GetRequiredService<ICommandHandler<SendMessageEverywhereCommand>>();
    }


    [Fact]
    public async void Send_message_everywhere()
    {
        //array   
        SendMessageEverywhereCommand command = new(
            UserId: Guid.NewGuid(),
            TelegramUserId: "telegram_user_123",
            Body: "Hello! This is a test message.",
            Subject: "Test Subject");
         
        //act 
        var result = await _sut.Execute(command, CancellationToken.None);

        //assert
        Assert.True(result.IsSuccess);
    }
}