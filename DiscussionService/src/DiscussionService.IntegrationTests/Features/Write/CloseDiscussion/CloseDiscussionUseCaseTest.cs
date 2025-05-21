using DiscussionService.Application.Features.Write.CloseDiscussion;
using Microsoft.Extensions.DependencyInjection;
using PetHome.Core.Application.Interfaces.FeatureManagement;
using PetHome.Discussions.IntegrationTests.IntegrationFactories;
using Xunit;

namespace PetHome.Discussions.Application.Features.Write.CloseDiscussion;
public class CloseDiscussionUseCaseTest : DiscussionFactory
{
    private readonly ICommandHandler<CloseDiscussionCommand> _sut;
    public CloseDiscussionUseCaseTest(IntegrationTestFactory factory) : base(factory)
    {
        _sut = _scope.ServiceProvider.GetRequiredService<ICommandHandler<CloseDiscussionCommand>>();
    }

    [Fact]
    public async void Close_discussion()
    {
        //array
        await SeedDiscussions(5);
        var discussion = _dbContext.Discussions.First(); 

        CloseDiscussionCommand command = new CloseDiscussionCommand(discussion.Id.Value);

        //act
        var result = await _sut.Execute(command, CancellationToken.None);

        //assert
        Assert.True(result.IsSuccess);
    }
}
