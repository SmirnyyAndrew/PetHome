using CSharpFunctionalExtensions;
using DiscussionService.Application.Features.Write.SendMessageInDiscussion;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
 using PetHome.Core.Application.Interfaces.FeatureManagement;
using PetHome.Discussions.IntegrationTests.IntegrationFactories;
using Xunit;

namespace PetHome.Discussions.Application.Features.Write.SendMessageInDiscussion;
public class SendMessageInDiscussionUseCaseTest : DiscussionFactory
{
    private readonly ICommandHandler<SendMessageInDiscussionCommand> _sut;
    public SendMessageInDiscussionUseCaseTest(IntegrationTestFactory factory) : base(factory)
    {
        _sut = _scope.ServiceProvider.GetRequiredService<ICommandHandler<SendMessageInDiscussionCommand>>();
    }

    [Fact]
    public async void Send_message_in_discussion()
    {
        //array
        await SeedDiscussions(5);
        var discussion = _dbContext.Discussions
            .Include(d=>d.Messages)
            .First();

        Guid discussionid = discussion.Id;
        Guid userId = discussion.UserIds.First();
        Guid messageId = discussion.Messages
            .Where(u => u.UserId == userId)
            .FirstOrDefault().Id;
        string message = "It's a message";

        SendMessageInDiscussionCommand command = new SendMessageInDiscussionCommand(discussionid, userId, message);

        //act
        var result = await _sut.Execute(command, CancellationToken.None);

        //assert
        Assert.True(result.IsSuccess);
    }
}
