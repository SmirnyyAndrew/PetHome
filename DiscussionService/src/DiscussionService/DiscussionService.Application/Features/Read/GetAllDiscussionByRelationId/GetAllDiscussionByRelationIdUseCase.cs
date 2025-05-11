using CSharpFunctionalExtensions;
using DiscussionService.Application.Database.Dto;
using DiscussionService.Application.Database.Interfaces;
using Microsoft.EntityFrameworkCore;
using PetHome.Core.Application.Interfaces.FeatureManagement;
using PetHome.Core.Domain.Models;
using PetHome.Core.Web.Extentions.Collection;
using PetHome.SharedKernel.Responses.ErrorManagement;


namespace DiscussionService.Application.Features.Read.GetAllDiscussionByRelationId;
public class GetAllDiscussionByRelationIdUseCase
    : IQueryHandler<PagedList<DiscussionDto>, GetAllDiscussionByRelationIdQuery>
{
    private readonly IDiscussionReadDbContext _readDbContext;

    public GetAllDiscussionByRelationIdUseCase(IDiscussionReadDbContext readDbContext)
    {
        _readDbContext = readDbContext;
    }

    public async Task<Result<PagedList<DiscussionDto>, ErrorList>> Execute(
        GetAllDiscussionByRelationIdQuery query, CancellationToken ct)
    { 
        var discussionsPagedList = await _readDbContext.Discussions
            .Include(d => d.Messages)
            .Where(d => d.RelationId == query.RelationId)
            .ToPagedList(query.PageNum, query.PageSize, ct);

        return discussionsPagedList;
    }
}
