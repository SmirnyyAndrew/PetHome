using CSharpFunctionalExtensions;
using DiscussionService.Application.Features.Write.RemoveMessageInDiscussion;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
 using PetHome.Core.Application.Interfaces.FeatureManagement;
using PetHome.Discussions.IntegrationTests.IntegrationFactories;
using Xunit;

namespace PetHome.Discussions.Application.Features.Write.RemoveMessageInDiscussion;
public class RemoveMessageInDiscussionUseCaseTest : DiscussionFactory
{
    private readonly ICommandHandler<RemoveMessageInDiscussionCommand> _sut;
    public RemoveMessageInDiscussionUseCaseTest(IntegrationTestFactory factory) : base(factory)
    {
        _sut = _scope.ServiceProvider.GetRequiredService<ICommandHandler<RemoveMessageInDiscussionCommand>>();
    }

    [Fact]
    public async void Remove_message_in_discussion()
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

        RemoveMessageInDiscussionCommand command = new RemoveMessageInDiscussionCommand(discussionid, userId, messageId);

        //act
        var result = await _sut.Execute(command, CancellationToken.None);

        //assert
        Assert.True(result.IsSuccess);
    }
}
