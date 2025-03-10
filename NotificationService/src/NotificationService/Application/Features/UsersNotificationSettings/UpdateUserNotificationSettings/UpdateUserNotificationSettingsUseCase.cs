using CSharpFunctionalExtensions;
using NotificationService.Infrastructure.Database;
using PetHome.Core.Interfaces.FeatureManagment;
using PetHome.Core.Response.Validation.Validator;

namespace NotificationService.Application.Features.UsersNotificationSettings.UpdateUserNotificationSettings;

public class UpdateUserNotificationSettingsUseCase(
    NotificationRepository repository, UnitOfWork unitOfWork) 
    : ICommandHandler<UpdateUserNotificationSettingsCommand>
{
    public async Task<UnitResult<ErrorList>> Execute(
        UpdateUserNotificationSettingsCommand command, CancellationToken ct)
    {
        await repository.Update(command.UserId,command.NewNotificationSettings, ct);
        await unitOfWork.SaveChanges(ct);

        return Result.Success<ErrorList>();
    }
}
