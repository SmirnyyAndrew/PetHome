using MassTransit;
using NotificationService.Application.Consumers.Accounts;
using NotificationService.Application.Consumers.Discussions;
using NotificationService.Application.Consumers.VolunteerRequests;
using PetHome.Core.Web.Options.MessageBus;

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
            config.AddConsumer<OpenDiscussionConsumer>();
            config.AddConsumer<CloseDiscussionConsumer>();
            config.AddConsumer<CreateDiscussionConsumer>();
            config.AddConsumer<SetVolunteerRequestApprovedConsumer>();
            config.AddConsumer<SetVolunteerRequestRejectedConsumer>();
            config.AddConsumer<SetVolunteerRequestSubmittedConsumer>();

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
