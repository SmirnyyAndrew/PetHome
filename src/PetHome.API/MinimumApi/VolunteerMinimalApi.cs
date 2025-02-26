using MassTransit;
using PetHome.Core.ValueObjects.MainInfo;
using PetHome.Core.ValueObjects.PetManagment.Extra;
using PetHome.Volunteers.Contracts.Messaging;

namespace PetHome.API.MinimumApi;

public static class VolunteerMinimalApi
{
    public static WebApplication CreateVolunteerApi(this WebApplication app)
    {
        app.MapPost("volunteer", async (IPublishEndpoint publisher) =>
        {
            await publisher.Publish<CreatedVolunteerEvent>(new CreatedVolunteerEvent(
                new FullNameDto("Ivan", "Ivanov"),
                "mail@mail.com",
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
