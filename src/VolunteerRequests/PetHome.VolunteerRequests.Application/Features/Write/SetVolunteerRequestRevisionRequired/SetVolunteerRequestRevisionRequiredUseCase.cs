using CSharpFunctionalExtensions;
using Microsoft.Extensions.DependencyInjection;
using PetHome.Core.Constants;
using PetHome.Core.Extentions.ErrorExtentions;
using PetHome.Core.Interfaces.FeatureManagment;
using PetHome.Core.Response.ErrorManagment;
using PetHome.Core.Response.Validation.Validator;
using PetHome.Core.ValueObjects.User;
using PetHome.Core.ValueObjects.VolunteerRequest;
using PetHome.Framework.Database;
using PetHome.VolunteerRequests.Application.Database.Interfaces;
using PetHome.VolunteerRequests.Domain;

namespace PetHome.VolunteerRequests.Application.Features.Write.SetVolunteerRequestRevisionRequired;
public class SetVolunteerRequestRevisionRequiredUseCase
    : ICommandHandler<SetVolunteerRequestRevisionRequiredCommand>
{

    private readonly IVolunteerRequestRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public SetVolunteerRequestRevisionRequiredUseCase(
        IVolunteerRequestRepository repository,
        [FromKeyedServices(Constants.VOLUNTEER_REQUEST_UNIT_OF_WORK_KEY)] IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<UnitResult<ErrorList>> Execute(
        SetVolunteerRequestRevisionRequiredCommand command, CancellationToken ct)
    {
        var volunteerRequest = await _repository.GetById(command.VolunteerRequestId, ct);
        if (volunteerRequest is null)
            return Errors.NotFound(nameof(VolunteerRequest)).ToErrorList();

        UserId adminId = UserId.Create(command.AdminId).Value;
        RequestComment comment = RequestComment.Create(command.RejectedComment).Value;
        volunteerRequest.SetRejected(adminId, comment);

        var transaction = await _unitOfWork.BeginTransaction(ct);
        _repository.Update(volunteerRequest);
        transaction.Commit();
        await _unitOfWork.SaveChanges(ct);

        return Result.Success<ErrorList>();
    }
}
