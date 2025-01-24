using Microsoft.Extensions.DependencyInjection;
using PetHome.Core.Interfaces.FeatureManagment;
using PetHome.Discussions.IntegrationTests.IntegrationFactories;
using Xunit;

namespace PetHome.Discussions.Application.Features.Write.OpenDiscussion;
public class OpenDiscussionUseCaseTest : DiscussionFactory
{
    private readonly ICommandHandler<OpenDiscussionCommand> _sut;
    public OpenDiscussionUseCaseTest(IntegrationTestFactory factory) : base(factory)
    {
        _sut = _scope.ServiceProvider.GetRequiredService<ICommandHandler<OpenDiscussionCommand>>();
    }

    [Fact]
    public async void Close_discussion()
    {
        //array
        await SeedDiscussions(5);
        var discussion = _dbContext.Discussions.First();

        OpenDiscussionCommand command = new OpenDiscussionCommand(discussion.Id.Value);

        //act
        var result = await _sut.Execute(command, CancellationToken.None);

        //assert
        Assert.True(result.IsSuccess);
    }
}
