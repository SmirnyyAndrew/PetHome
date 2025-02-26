using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PetHome.Discussions.Application.Features.Consumers;
using PetHome.SharedKernel.Options.Volunteers;
using PetHome.Species.Application.Features.Consumers;
using PetHome.VolunteerRequests.Application.Features.Consumers;
using PetHome.Volunteers.Application.Features.Consumers;
using PetHome.Volunteers.Infrastructure.DI;

namespace PetHome.API.DependencyInjections.AppExtentions;
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
            config.AddConsumer<CreateVolunteerConsumer>();
            config.AddConsumer<CreateSpeciesConsumer>();
            config.AddConsumer<CreateDiscussionConsumer>();
            config.AddConsumer<CreateVolunteerRequestConsumer>();

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
