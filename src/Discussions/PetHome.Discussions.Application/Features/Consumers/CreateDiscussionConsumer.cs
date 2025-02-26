using MassTransit;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PetHome.Core.Constants;
using PetHome.Core.ValueObjects.Discussion.Relation;
using PetHome.Core.ValueObjects.User;
using PetHome.Discussions.Application.Database.Interfaces;
using PetHome.Discussions.Contracts.Messaging;
using PetHome.Discussions.Domain;
using PetHome.Framework.Database;

namespace PetHome.Discussions.Application.Features.Consumers;
public class CreateDiscussionConsumer : IConsumer<CreatedDiscussionEvent>
{
    private readonly IDiscussionRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<CreateDiscussionConsumer> _logger;

    public CreateDiscussionConsumer(
        IDiscussionRepository repository,
        ILogger<CreateDiscussionConsumer> logger,
        [FromKeyedServices(Constants.DISCUSSION_UNIT_OF_WORK_KEY)] IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public async Task Consume(ConsumeContext<CreatedDiscussionEvent> context)
    {
        var command = context.Message;

        if (command.UsersIds.Count() > 2)
        {
            _logger.LogError("Пользователей не должно быть больше 2");
            return;
        }

        RelationId relationId = RelationId.Create(
            command.RelationId == default ? Guid.NewGuid() : command.RelationId).Value;

        List<UserId> usersIds = command.UsersIds
            .Select(u => UserId.Create(u).Value)
            .ToList();
        Discussion discussion = Discussion.Create(relationId, usersIds).Value;

        var transaction = await _unitOfWork.BeginTransaction(CancellationToken.None);
        await _repository.AddDiscussion(discussion);
        await _unitOfWork.SaveChanges(CancellationToken.None);
        transaction.Commit();

        return;
    }
}
