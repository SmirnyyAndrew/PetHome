using CSharpFunctionalExtensions;
using Microsoft.Extensions.DependencyInjection;
using PetHome.Core.Constants;
using PetHome.Core.Interfaces.FeatureManagment;
using PetHome.Core.Response.Validation.Validator;
using PetHome.Core.ValueObjects.User;
using PetHome.Core.ValueObjects.VolunteerRequest;
using PetHome.Framework.Database;
using PetHome.VolunteerRequests.Application.Database.Interfaces;
using PetHome.VolunteerRequests.Domain;

namespace PetHome.VolunteerRequests.Application.Features.Write.CreateVolunteerRequests;
public class CreateVolunteerRequestsUseCase
    : ICommandHandler<CreateVolunteerRequestsCommand>
{
    private readonly IVolunteerRequestRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateVolunteerRequestsUseCase(
        IVolunteerRequestRepository repository,
        [FromKeyedServices(Constants.VOLUNTEER_UNIT_OF_WORK_KEY)] IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<UnitResult<ErrorList>> Execute(
        CreateVolunteerRequestsCommand command, CancellationToken ct)
    {
        UserId userId = UserId.Create(command.UserId).Value;
        VolunteerInfo volunteerInfo = VolunteerInfo.Create(command.VolunteerInfo).Value;
        VolunteerRequest request = VolunteerRequest.Create(userId, volunteerInfo);

        var transaction = await _unitOfWork.BeginTransaction(ct);
        await _repository.Add(request);
        transaction.Commit();
        await _unitOfWork.SaveChanges(ct);
        
        return Result.Success<ErrorList>();
    }
}
