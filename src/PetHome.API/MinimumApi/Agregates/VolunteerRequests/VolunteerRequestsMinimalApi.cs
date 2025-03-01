using MassTransit;
using PetHome.VolunteerRequests.Contracts.Messaging;

namespace PetHome.API.MinimumApi.Agregates.VolunteerRequests;

public static class VolunteerRequestsMinimalApi
{
    public static WebApplication CreateVolunteerRequests(this WebApplication app)
    {

        app.MapPost("volunteer-request", async (IPublishEndpoint publisher) =>
        {
            await publisher.Publish(new CreatedVolunteerRequestEvent(
               Guid.NewGuid(),
               Guid.NewGuid(),
               "Some info"
                ));
        });

        return app;
    }
}
