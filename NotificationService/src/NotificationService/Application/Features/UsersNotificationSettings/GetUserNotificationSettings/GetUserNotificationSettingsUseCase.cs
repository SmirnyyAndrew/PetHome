using CSharpFunctionalExtensions;
using NotificationService.Domain;
using NotificationService.Infrastructure.Database;
using PetHome.Core.Interfaces.FeatureManagment;
using PetHome.Core.Response.Validation.Validator;

namespace NotificationService.Application.Features.UsersNotificationSettings.GetUserNotificationSettings;

public class GetUserNotificationSettingsUseCase(NotificationRepository repository)
    : IQueryHandler<UserNotificationSettings?, GetUserNotificationSettingsQuery>
{
    public async Task<Result<UserNotificationSettings?, ErrorList>> Execute(
        GetUserNotificationSettingsQuery query, CancellationToken ct)
    {
        return await repository.Get(query.UserId, ct);
    } 
}
