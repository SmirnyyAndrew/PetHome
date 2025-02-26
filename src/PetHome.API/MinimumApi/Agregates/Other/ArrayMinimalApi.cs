namespace PetHome.API.MinimumApi.Agregates.Other;

public static class ArrayMinimalApi
{
    public static WebApplication GetStringsArrayApi(this WebApplication app)
    {
        app.MapGet("strings-array", () =>
        {
            return new string[] { "str1", "str2", "str3", "str4", "str5" };
        });

        return app;
    }
}
