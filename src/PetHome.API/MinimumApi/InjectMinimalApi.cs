using PetHome.API.MinimumApi.Agregates.Discussions;
using PetHome.API.MinimumApi.Agregates.Other;
using PetHome.API.MinimumApi.Agregates.Species;
using PetHome.API.MinimumApi.Agregates.VolunteerRequests;
using PetHome.API.MinimumApi.Agregates.Volunteers;

namespace PetHome.API.MinimumApi;

public static class InjectMinimalApi
{
    public static WebApplication AddMinimalApi(this WebApplication app)
    {
        app.GetStringsArrayApi();
        app.CreateVolunteerApi();
        app.CreateSpeciesApi();
        app.CreateDiscussion();
        app.CreateVolunteerRequests();

        return app;
    }
}
