using NotificationService.Application.Features.GetAnyUsersNotificationSettings;
using NotificationService.Application.Features.GetUserNotificationSettings;
using NotificationService.Application.Features.GetUsersEmailSendings;
using NotificationService.Application.Features.GetUsersTelegramSendings;
using NotificationService.Application.Features.GetUsersWebSendings;
using NotificationService.Application.Features.ResetUserNotificationSettings;
using NotificationService.Application.Features.UpdateUserNotificationSettings;

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

        return services;
    }
}
