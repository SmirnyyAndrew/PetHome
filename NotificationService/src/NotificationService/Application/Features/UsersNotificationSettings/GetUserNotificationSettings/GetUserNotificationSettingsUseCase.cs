using CSharpFunctionalExtensions;
using NotificationService.Domain;
using NotificationService.Infrastructure.Database;
using PetHome.Core.Application.Interfaces.FeatureManagement;
using PetHome.SharedKernel.Responses.ErrorManagement;

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
