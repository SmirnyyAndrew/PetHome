using MassTransit;
using PetHome.Discussions.Contracts.Messaging;

namespace PetHome.API.MinimumApi.Agregates.Discussions;

public static class DiscussionMinimalApi
{
    public static WebApplication CreateDiscussion(this WebApplication app)
    {

        app.MapPost("discussion", async (IPublishEndpoint publisher) =>
        {
            int randomInt = new Random().Next(100);

            await publisher.Publish(new CreatedDiscussionEvent(
                [Guid.NewGuid(), Guid.NewGuid()],
                $"relation name №{randomInt}"
                ));
        });

        return app;
    }
}
