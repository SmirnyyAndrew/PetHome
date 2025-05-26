using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Environments = PetHome.Core.Tests.IntegrationTests.Constants.Environments;

namespace PetHome.Core.Tests.IntegrationTests.DependencyInjections;
public static class EnvironmentExtentions
{
    public static IWebHostBuilder AddEnvironment(
        this IWebHostBuilder builder, Environments environment = Environments.Test)
    {
        builder.UseEnvironment(environment.ToString());

        return builder;
    }


    public static bool IsEnvironment(
        this IHostEnvironment env, Environments environment = Environments.Test)
    {
        return env.IsEnvironment(environment.ToString());
    }

    public static bool IsTestEnvironment(this IHostEnvironment env)
    {
        return env.IsEnvironment(Environments.Test);
    }

}
