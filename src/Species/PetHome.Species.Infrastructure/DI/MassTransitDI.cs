using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PetHome.SharedKernel.Options.Volunteers;
using PetHome.Species.Application.Features.Consumers;
using PetHome.Species.Contracts.Messaging;
using PetHome.Species.Infrastructure.DI;

namespace PetHome.Species.Infrastructure.DI;
public static class MassTransitDI
{
    public static IServiceCollection AddMassTransit(
        this IServiceCollection services, IConfiguration configuration)
    {
        RabbitMqOption option = configuration.GetSection(RabbitMqOption.SECTION_NAME).Get<RabbitMqOption>()!;

        services.AddMassTransit(config =>
        {
            config.SetKebabCaseEndpointNameFormatter();

            //Consumers 
            config.AddConsumer<CreateSpeciesConsumer>();

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
