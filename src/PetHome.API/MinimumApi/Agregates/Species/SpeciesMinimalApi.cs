using MassTransit;
using PetHome.Species.Contracts.Messaging;

namespace PetHome.API.MinimumApi.Agregates.Species;

public static class SpeciesMinimalApi
{
    public static WebApplication CreateSpeciesApi(this WebApplication app)
    {

        app.MapPost("species", async (IPublishEndpoint publisher) =>
        {
            int randomInt = new Random().Next(1000);
            await publisher.Publish(new CreatedSpeciesEvent(
                $"species №{randomInt}"
                ));
        });

        return app;
    }
}
