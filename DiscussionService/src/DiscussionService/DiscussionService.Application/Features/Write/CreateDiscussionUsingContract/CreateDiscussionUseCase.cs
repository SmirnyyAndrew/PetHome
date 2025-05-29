using CSharpFunctionalExtensions;
using DiscussionService.Application.Database.Interfaces;
using DiscussionService.Contracts.Messaging;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PetHome.Core.Application.Interfaces.FeatureManagement;
using PetHome.Core.Infrastructure.Database;
using PetHome.Core.Tests.IntegrationTests.DependencyInjections;
using PetHome.Core.Web.Extentions.ErrorExtentions;
using PetHome.Discussions.Domain;
using PetHome.SharedKernel.Constants;
using PetHome.SharedKernel.Responses.ErrorManagement;
using PetHome.SharedKernel.ValueObjects.Discussion;
using PetHome.SharedKernel.ValueObjects.Discussion.Relation;
using PetHome.SharedKernel.ValueObjects.User;

namespace DiscussionService.Application.Features.Write.CreateDiscussionUsingContract;
public class CreateDiscussionUseCase
    : ICommandHandler<DiscussionId, CreateDiscussionCommand>
{
    private readonly IDiscussionRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPublishEndpoint _publisher;
    private readonly IHostEnvironment _env;


    public CreateDiscussionUseCase(
        IDiscussionRepository repository,
        IPublishEndpoint publisher,
        IHostEnvironment env,
        [FromKeyedServices(Constants.Database.DISCUSSION_UNIT_OF_WORK_KEY)] IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _publisher = publisher;
        _env = env;
    }

    public async Task<Result<DiscussionId, ErrorList>> Execute(
        CreateDiscussionCommand command, CancellationToken ct)
    {
        var transaction = await _unitOfWork.BeginTransaction(ct);
        //if (command.UsersIds.Count() > 2)
        //    return Errors.Validation("Пользователей не должно быть больше 2").ToErrorList();

        RelationId relationId = RelationId.Create(command.RelationId).Value;
        List<UserId> usersIds = command.UsersIds
            .Select(u => UserId.Create(u).Value)
            .ToList();
        Relation relation = await _repository.GetRelationById(command.RelationId, ct);

        Discussion discussion = Discussion.Create(relationId, usersIds).Value;
        Discussion discussion2 = await _repository.GetDiscussionById(discussion.Id, ct);

        await _repository.AddDiscussion(discussion);
        await _unitOfWork.SaveChanges(ct);

        if (!_env.IsTestEnvironment())
        {
            CreatedDiscussionEvent createdDiscussionEvent = new CreatedDiscussionEvent(
                discussion.Id,
                 relation?.Id ?? Guid.Empty,
                relation?.Name ?? String.Empty,
                discussion.UserIds.Select(u => u.Value));
            await _publisher.Publish(createdDiscussionEvent, ct);
        }

        transaction.Commit();

        return discussion.Id;
    }
}
