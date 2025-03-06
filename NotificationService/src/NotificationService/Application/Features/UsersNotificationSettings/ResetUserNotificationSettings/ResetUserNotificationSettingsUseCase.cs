using CSharpFunctionalExtensions;
using NotificationService.Domain;
using NotificationService.Infrastructure.Database;
using PetHome.Core.Interfaces.FeatureManagment;
using PetHome.Core.Response.Validation.Validator;

namespace NotificationService.Application.Features.UsersNotificationSettings.ResetUserNotificationSettings;

public class ResetUserNotificationSettingsUseCase(
    NotificationRepository repository, UnitOfWork unitOfWork) 
    : ICommandHandler<ResetUserNotificationSettingsCommand>
{
    public async Task<UnitResult<ErrorList>> Execute(
        ResetUserNotificationSettingsCommand command, CancellationToken ct)
    {
        await repository.Reset(command.UserId, ct);
        await unitOfWork.SaveChanges(ct);

        return Result.Success<ErrorList>();
    }
}
