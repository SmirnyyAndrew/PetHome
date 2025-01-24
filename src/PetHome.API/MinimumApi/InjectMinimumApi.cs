namespace PetHome.API.MinimumApi;

public static class InjectMinimumApi
{
    public static WebApplication AddMinimumApi(this WebApplication app)
    {
        app.GetStringsArray();

        return app;
    }
}
