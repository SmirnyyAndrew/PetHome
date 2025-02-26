using MassTransit;
using PetHome.Discussions.Contracts.Messaging;

namespace PetHome.API.MinimumApi;

public static class DiscussionMinimalApi
{
    public static WebApplication CreateDiscussion(this WebApplication app)
    {

        app.MapPost("discussion", async (IPublishEndpoint publisher) =>
        { 
            await publisher.Publish(new CreatedDiscussionEvent(
                [Guid.NewGuid(), Guid.NewGuid()]
                ));
        });

        return app;
    }
}
