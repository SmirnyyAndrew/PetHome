namespace PetHome.API.MinimumApi;

public static class ArrayMinimumApi
{
    public static WebApplication GetStringsArray(this WebApplication app)
    {
        app.MapGet("strings-array", () =>
        {
            return new string[] {"str1", "str2", "str3", "str4", "str5" };
        });

        return app;
    }
}
