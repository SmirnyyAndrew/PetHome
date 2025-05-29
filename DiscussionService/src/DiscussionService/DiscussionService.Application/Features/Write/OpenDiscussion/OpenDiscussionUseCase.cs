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

namespace DiscussionService.Application.Features.Write.OpenDiscussion;
public class OpenDiscussionUseCase
    : ICommandHandler<OpenDiscussionCommand>
{
    private readonly IDiscussionRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPublishEndpoint _publisher;
    private readonly IHostEnvironment _env;

    public OpenDiscussionUseCase(
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

        if (!_env.IsTestEnvironment())
        {
            OpenedDiscussionEvent openedDiscussionEvent = new OpenedDiscussionEvent(
                discussion.Id,
                discussion.RelationId,
                discussion.Relation?.Name,
                discussion.UserIds.Select(u => u.Value));
            await _publisher.Publish(openedDiscussionEvent, ct);
        }

        transaction.Commit();

        return Result.Success<ErrorList>();
    }
}
