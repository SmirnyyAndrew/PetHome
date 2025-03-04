using NotificationService.Application.Features.Email.SendMessage;
using NotificationService.Application.Features.UsersNotificationSettings.GetAnyUsersNotificationSettings;
using NotificationService.Application.Features.UsersNotificationSettings.GetUserNotificationSettings;
using NotificationService.Application.Features.UsersNotificationSettings.GetUsersEmailSendings;
using NotificationService.Application.Features.UsersNotificationSettings.GetUsersTelegramSendings;
using NotificationService.Application.Features.UsersNotificationSettings.GetUsersWebSendings;
using NotificationService.Application.Features.UsersNotificationSettings.ResetUserNotificationSettings;
using NotificationService.Application.Features.UsersNotificationSettings.UpdateUserNotificationSettings;

namespace NotificationService.DependencyInjections.ApplicationDependencyInjections;

public static class ServicesDependencyInjections
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<GetAnyUsersNotificationSettingsUseCase>();
        services.AddScoped<GetUserNotificationSettingsUseCase>();
        services.AddScoped<GetUsersEmailSendingsUseCase>();
        services.AddScoped<GetUsersTelegramSendingsUseCase>();
        services.AddScoped<GetUsersWebSendingsUseCase>();
        services.AddScoped<ResetUserNotificationSettingsUseCase>();
        services.AddScoped<UpdateUserNotificationSettingsUseCase>();
     
        services.AddScoped<SendMessageUseCase>();

        return services;
    }
}
