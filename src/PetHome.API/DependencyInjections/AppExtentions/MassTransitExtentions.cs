using MassTransit;
using PetHome.Accounts.Application.Features.Consumers.UserManagement.CreateAdmin;
using PetHome.Accounts.Application.Features.Consumers.UserManagement.CreateParticipant;
using PetHome.Accounts.Application.Features.Consumers.UserManagement.CreateUser;
using PetHome.Discussions.Application.Features.Consumers;
using PetHome.SharedKernel.Options.Volunteers;
using PetHome.Species.Application.Features.Consumers;
using PetHome.VolunteerRequests.Application.Features.Consumers;
using PetHome.Volunteers.Application.Features.Consumers;

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
            config.AddConsumer<CreateAdminConsumer>();
            config.AddConsumer<CreateParticipantConsumer>();
            config.AddConsumer<CreateUserConsumer>();
            config.AddConsumer<CreateVolunteerConsumer>();

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
