using CSharpFunctionalExtensions;
using Microsoft.Extensions.DependencyInjection;
using PetHome.Core.Constants;
using PetHome.Core.Extentions.ErrorExtentions;
using PetHome.Core.Interfaces.FeatureManagment;
using PetHome.Core.Response.ErrorManagment;
using PetHome.Core.Response.Validation.Validator;
using PetHome.Core.ValueObjects.Discussion.Message;
using PetHome.Core.ValueObjects.User;
using PetHome.Discussions.Application.Database.Interfaces;
using PetHome.Discussions.Domain;
using PetHome.Framework.Database;

namespace PetHome.Discussions.Application.Features.Write.RemoveMessageInDiscussion;
public class RemoveMessageInDiscussionUseCase
    : ICommandHandler<RemoveMessageInDiscussionCommand>
{
    private readonly IDiscussionRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public RemoveMessageInDiscussionUseCase(
        IDiscussionRepository repository,
        [FromKeyedServices(Constants.DISCUSSION_UNIT_OF_WORK_KEY)] IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<UnitResult<ErrorList>> Execute(
        RemoveMessageInDiscussionCommand command, CancellationToken ct)
    {
        Discussion? discussion = await _repository.GetDiscussionById(command.DiscussionId, ct);
        if (discussion is null)
            return Errors.NotFound($"Discussion с id = {command.DiscussionId}").ToErrorList();

        UserId userId = UserId.Create(command.UserId).Value;
        bool userIsParticipantOfDiscussion = discussion.UserIds.Contains(userId);
        if (userIsParticipantOfDiscussion is not true)
            return Errors.Failure($"User с id = {command.DiscussionId} не является участником дискуссии").ToErrorList();

        MessageId messageId = MessageId.Create(command.MessageId).Value;
        Message? message = discussion.Messages.FirstOrDefault(m => m.Id == command.MessageId);
        if (message is null)
            return Errors.NotFound($"Message с id = {command.MessageId}").ToErrorList();

        discussion.RemoveMessage(message);

        var transaction = await _unitOfWork.BeginTransaction(ct);
        await _repository.UpdateDiscussion(discussion);
        transaction.Commit();
        await _unitOfWork.SaveChanges(ct);

        return Result.Success<ErrorList>();
    }
}
