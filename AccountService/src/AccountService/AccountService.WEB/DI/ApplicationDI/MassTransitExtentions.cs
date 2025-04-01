using MassTransit;
using PetHome.SharedKernel.Options.Volunteers;

namespace AccountService.WEB.DI.ApplicationDI;
public static class MassTransitExtentions
{
    public static IServiceCollection AddMassTransitConfig(
        this IServiceCollection services, IConfiguration configuration)
    {
        RabbitMqOption option = configuration.GetSection(RabbitMqOption.SECTION_NAME).Get<RabbitMqOption>()!;

        services.AddMassTransit(config =>
        {
            config.SetKebabCaseEndpointNameFormatter();

            //Consumers  

            config.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(new Uri(option.Host),
                    h =>
                    {
                        h.Username(option.Username);
                        h.Password(option.Password);
                    });
                cfg.Durable = true;
                cfg.ConfigureEndpoints(context);
            });
        });

        return services;
    }
}
