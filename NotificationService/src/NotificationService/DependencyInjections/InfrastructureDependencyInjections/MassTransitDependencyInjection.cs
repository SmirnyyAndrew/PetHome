using MassTransit;
using NotificationService.Application.Consumers;
using PetHome.SharedKernel.Options.Volunteers;

namespace NotificationService.DependencyInjections.InfrastructureDependencyInjections;

public static class MassTransitDependencyInjection
{
    public static IServiceCollection AddMassTransitConfig(
        this IServiceCollection services, IConfiguration configuration)
    {
        RabbitMqOption option = configuration.GetSection(RabbitMqOption.SECTION_NAME).Get<RabbitMqOption>()!;

        services.AddMassTransit(config =>
        {
            config.SetKebabCaseEndpointNameFormatter();

            //Consumers 
            config.AddConsumer<ConfirmUserEmailConsumer>(); 
            config.AddConsumer<CreateUserConsumer>(); 

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
