namespace PetHome.API.MinimumApi;

public static class InjectMinimalApi
{
    public static WebApplication AddMinimalApi(this WebApplication app)
    {
        app.GetStringsArrayApi();
        app.CreateVolunteerApi();
        app.CreateSpeciesApi();

        return app;
    }
}
