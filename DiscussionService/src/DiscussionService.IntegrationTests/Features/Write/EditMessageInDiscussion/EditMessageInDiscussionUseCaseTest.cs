using CSharpFunctionalExtensions;
using DiscussionService.Application.Features.Write.EditMessageInDiscussion;
using Microsoft.Extensions.DependencyInjection;
 using PetHome.Core.Application.Interfaces.FeatureManagement;
using PetHome.Discussions.IntegrationTests.IntegrationFactories;
using Xunit;

namespace PetHome.Discussions.Application.Features.Write.EditMessageInDiscussion;
public class EditMessageInDiscussionUseCaseTest : DiscussionFactory
{
    private readonly ICommandHandler<EditMessageInDiscussionCommand> _sut;
    public EditMessageInDiscussionUseCaseTest(IntegrationTestFactory factory) : base(factory)
    {
        _sut = _scope.ServiceProvider.GetRequiredService<ICommandHandler<EditMessageInDiscussionCommand>>();
    }

    [Fact]
    public async void Edit_message_in_discussion()
    {
        //array
        await SeedDiscussions(5);
        var discussion = _dbContext.Discussions.First();
        Guid discussionid = discussion.Id;
        Guid userId = discussion.UserIds.First();
        Guid messageId = discussion.Messages
            .Where(u => u.UserId == userId)
            .FirstOrDefault().Id;
        string newMessage = "It's new message";

        EditMessageInDiscussionCommand command = new EditMessageInDiscussionCommand(
            discussionid,
            userId,
            messageId,
            newMessage);

        //act
        var result = await _sut.Execute(command, CancellationToken.None);

        //assert
        Assert.True(result.IsSuccess);
    }
}
