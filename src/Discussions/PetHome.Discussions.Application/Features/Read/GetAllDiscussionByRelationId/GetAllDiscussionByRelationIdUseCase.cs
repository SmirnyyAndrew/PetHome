using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using PetHome.Core.Extentions.Collection;
using PetHome.Core.Interfaces.FeatureManagment;
using PetHome.Core.Models;
using PetHome.Core.Response.Validation.Validator;
using PetHome.Discussions.Application.Database.Dto;
using PetHome.Discussions.Application.Database.Interfaces;

namespace PetHome.Discussions.Application.Features.Read.GetAllDiscussionByRelationId;
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
        List<DiscussionDto> discussions = await _readDbContext.Discussions
            .Include(d => d.Messages)
            .Where(d => d.RelationId == query.RelationId)
            .ToListAsync(ct);

        var discussionsPagedList = await discussions.AsQueryable()
            .ToPagedList(query.PageNum, query.PageSize, ct);

        return discussionsPagedList;
    }
}
