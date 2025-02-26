using MassTransit;
using PetHome.Core.ValueObjects.MainInfo;
using PetHome.Core.ValueObjects.PetManagment.Extra;
using PetHome.Volunteers.Contracts.Messaging;

namespace PetHome.API.MinimumApi.Agregates.Volunteers;

public static class VolunteerMinimalApi
{
    public static WebApplication CreateVolunteerApi(this WebApplication app)
    {
        app.MapPost("volunteer", async (IPublishEndpoint publisher) =>
        {
            int randomInt = new Random().Next(1000);

            await publisher.Publish(new CreatedVolunteerEvent(
                new FullNameDto($"Ivan{randomInt}", $"Ivanov{randomInt}"),
                $"mail{randomInt}@mail.com",
                "desc",
                Date.Create().Value,
                ["984545454"],
                ["vk.com"],
                [new RequisitesesDto("name", "desc", PaymentMethodEnum.Cash)]
                ));
        });

        return app;
    }
}
