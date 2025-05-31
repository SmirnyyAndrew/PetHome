using Microsoft.Extensions.DependencyInjection;
using NotificationService.Application.Features.Email.SendMessage;
using NotificationService.Domain;
using NotificationService.IntegrationTests.IntegrationFactories;
using PetHome.Core.Application.Interfaces.FeatureManagement;
using Xunit;

namespace NotificationService.IntegrationTests.Features.Email.SendMessageTest;
 
public class SendMessageTest : NotificationFactory
{
    private readonly ICommandHandler<SendEmailMessageCommand> _sut;

    public SendMessageTest(IntegrationTestFactory factory) : base(factory)
    {
        _sut = _scope.ServiceProvider.GetRequiredService<ICommandHandler<SendEmailMessageCommand>>();
    }


    [Fact]
    public async void Send_message_on_email()
    {
        //array  
        SendEmailMessageCommand command = new( 
            SenderEmailType: Emails.Yandex,
            RecipientEmail: "recipient@example.com",
            Subject: "Welcome to our service!",
            Body: "Hello,\nThank you for joining us."
        );

        //act 
        var result = await _sut.Execute(command, CancellationToken.None);

        //assert
        Assert.True(result.IsSuccess);
    }
}