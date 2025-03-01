using CSharpFunctionalExtensions;
using Microsoft.Extensions.DependencyInjection;
using PetHome.Core.Constants;
using PetHome.Core.Extentions.ErrorExtentions;
using PetHome.Core.Interfaces.FeatureManagment;
using PetHome.Core.Response.ErrorManagment;
using PetHome.Core.Response.Validation.Validator;
using PetHome.Discussions.Application.Database.Interfaces;
using PetHome.Discussions.Domain;
using PetHome.Framework.Database;

namespace PetHome.Discussions.Application.Features.Write.OpenDiscussion;
public class OpenDiscussionUseCase
    : ICommandHandler<OpenDiscussionCommand>
{
    private readonly IDiscussionRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public OpenDiscussionUseCase(
        IDiscussionRepository repository,
        [FromKeyedServices(Constants.DISCUSSION_UNIT_OF_WORK_KEY)] IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<UnitResult<ErrorList>> Execute(
        OpenDiscussionCommand command, CancellationToken ct)
    {
        Discussion? discussion = await _repository.GetDiscussionById(command.DiscussionId, ct);
        if (discussion is null)
            return Errors.NotFound($"Discussion с id = {command.DiscussionId}").ToErrorList();

        discussion.ReOpen();

        var transaction = await _unitOfWork.BeginTransaction(ct);
        await _repository.UpdateDiscussion(discussion);
        await _unitOfWork.SaveChanges(ct);
        transaction.Commit();

        return Result.Success<ErrorList>();
    }
}
