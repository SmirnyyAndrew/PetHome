using CSharpFunctionalExtensions;
using DiscussionService.Application.Database.Interfaces;
using DiscussionService.Contracts.Messaging;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using PetHome.Core.Constants;
using PetHome.Core.Extentions.ErrorExtentions;
using PetHome.Core.Interfaces.FeatureManagment;
using PetHome.Core.Response.ErrorManagment;
using PetHome.Core.Response.Validation.Validator;
using PetHome.Core.ValueObjects.Discussion.Message;
using PetHome.Core.ValueObjects.User;
using PetHome.Discussions.Domain;
using PetHome.Framework.Database;

namespace DiscussionService.Application.Features.Write.SendMessageInDiscussion;
public class SendMessageInDiscussionUseCase
    : ICommandHandler<SendMessageInDiscussionCommand>
{
    private readonly IDiscussionRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPublishEndpoint _publisher;

    public SendMessageInDiscussionUseCase(
        IDiscussionRepository repository,
        IPublishEndpoint publisher,
        [FromKeyedServices(Constants.DISCUSSION_UNIT_OF_WORK_KEY)] IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _publisher = publisher;
    }

    public async Task<UnitResult<ErrorList>> Execute(
        SendMessageInDiscussionCommand command, CancellationToken ct)
    {
        Discussion? discussion = await _repository.GetDiscussionById(command.DiscussionId, ct);
        if (discussion is null)
            return Errors.NotFound($"Discussion с id = {command.DiscussionId}").ToErrorList();

        UserId userId = UserId.Create(command.UserId).Value;
        bool userIsParticipantOfDiscussion = discussion.UserIds.Contains(userId);
        if (userIsParticipantOfDiscussion is not true)
            return Errors.Failure($"User с id = {command.DiscussionId} не является участником дискуссии").ToErrorList();

        MessageText text = MessageText.Create(command.Message).Value;
        Message message = Message.Create(text, userId);
        discussion.AddMessage(message);

        var transaction = await _unitOfWork.BeginTransaction(ct);
        await _repository.UpdateDiscussion(discussion);
        await _unitOfWork.SaveChanges(ct);

        SentMessageInDiscussionEvent sentMessageInDiscussionEvent = new SentMessageInDiscussionEvent(
            discussion.Id,
            userId,
            message?.Text);
        await _publisher.Publish(sentMessageInDiscussionEvent, ct);
       
        transaction.Commit();

        return Result.Success<ErrorList>();
    }
}
