using NotificationService.Application.Features.Email.SendMessage;
using NotificationService.Application.Features.GeneralNotification.SendMessageEverywhere;
using NotificationService.Application.Features.Telegram.RegisterUserTelegramAccount;
using NotificationService.Application.Features.Telegram.SendMessage;
using NotificationService.Application.Features.UsersNotificationSettings.GetAnyUsersNotificationSettings;
using NotificationService.Application.Features.UsersNotificationSettings.GetUserNotificationSettings;
using NotificationService.Application.Features.UsersNotificationSettings.GetUsersEmailSendings;
using NotificationService.Application.Features.UsersNotificationSettings.GetUsersTelegramSendings;
using NotificationService.Application.Features.UsersNotificationSettings.GetUsersWebSendings;
using NotificationService.Application.Features.UsersNotificationSettings.ResetUserNotificationSettings;
using NotificationService.Application.Features.UsersNotificationSettings.UpdateUserNotificationSettings;
using PetHome.Core.Interfaces.FeatureManagment;

namespace NotificationService.DependencyInjections.ApplicationDependencyInjections;

public static class ServicesDependencyInjections
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.Scan(scan => scan.FromAssemblies(typeof(ServicesDependencyInjections).Assembly)
        .AddClasses(classes => classes
            .AssignableToAny(
                typeof(ICommandHandler<>), typeof(ICommandHandler<,>),
                typeof(IQueryHandler<>), typeof(IQueryHandler<,>)))
        .AsSelfWithInterfaces()
        .WithScopedLifetime());

        return services;
    }
}
