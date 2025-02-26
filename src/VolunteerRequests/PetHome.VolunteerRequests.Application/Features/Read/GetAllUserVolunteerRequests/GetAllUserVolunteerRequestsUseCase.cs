using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using PetHome.Core.Extentions.Collection;
using PetHome.Core.Interfaces.FeatureManagment;
using PetHome.Core.Response.Validation.Validator;
using PetHome.VolunteerRequests.Application.Database.Dto;
using PetHome.VolunteerRequests.Application.Database.Interfaces;

namespace PetHome.VolunteerRequests.Application.Features.Read.GetAllUserVolunteerRequests;
public class GetAllUserVolunteerRequestsUseCase
    : IQueryHandler<IReadOnlyList<VolunteerRequestDto>, GetAllUserVolunteerRequestsQuery>
{
    private readonly IVolunteerRequestReadDbContext _readDbContext;

    public GetAllUserVolunteerRequestsUseCase(
        IVolunteerRequestReadDbContext readDbContext)
    {
        _readDbContext = readDbContext;
    }
    public async Task<Result<IReadOnlyList<VolunteerRequestDto>, ErrorList>> Execute(
        GetAllUserVolunteerRequestsQuery query, CancellationToken ct)
    { 
        var volunteerRequestsPagedList = await _readDbContext.VolunteerRequests
            .Where(r => r.UserId == query.UserId)
            .ToPagedList(query.PageNum, query.PageSize, ct);

        return volunteerRequestsPagedList;
    }
}
