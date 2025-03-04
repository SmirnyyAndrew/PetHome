using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using PetHome.Core.Extentions.Collection;
using PetHome.Core.Interfaces.FeatureManagment;
using PetHome.Core.Models;
using PetHome.Core.Response.Validation.Validator;
using PetHome.VolunteerRequests.Application.Database.Dto;
using PetHome.VolunteerRequests.Application.Database.Interfaces;

namespace PetHome.VolunteerRequests.Application.Features.Read.GetAllSubmittedVolunteerRequests;
public class GetAllSubmittedVolunteerRequestsUseCase
    : IQueryHandler<PagedList<VolunteerRequestDto>, GetAllSubmittedVolunteerRequestsQuery>
{  
    private readonly IVolunteerRequestReadDbContext _readDbContext;

    public GetAllSubmittedVolunteerRequestsUseCase(
        IVolunteerRequestReadDbContext readDbContext)
    {
        _readDbContext = readDbContext;
    } 

    public async Task<Result<PagedList<VolunteerRequestDto>, ErrorList>> Execute(
        GetAllSubmittedVolunteerRequestsQuery query, CancellationToken ct)
    { 
        var volunteerRequestsPagedList = await _readDbContext.VolunteerRequests
            .ToPagedList(query.PageNum, query.PageSize,ct);

        return volunteerRequestsPagedList;
    }
}
