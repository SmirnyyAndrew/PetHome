using MassTransit;
using PetHome.Species.Contracts.Messaging;

namespace PetHome.API.MinimumApi;

public static class SpeciesMinimalApi
{
    public static WebApplication CreateSpeciesApi(this WebApplication app)
    {

        app.MapPost("species", async (IPublishEndpoint publisher) =>
        {
            int randomInt = new Random().Next(1000);
            await publisher.Publish<CreatedSpeciesEvent>(new CreatedSpeciesEvent(
                $"species №{randomInt}"
                ));
        });

        return app;
    }
}
