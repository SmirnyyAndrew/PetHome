using CSharpFunctionalExtensions;
using PetHome.Core.Application.Interfaces.FeatureManagement;
using PetHome.Core.Domain.Models;
using PetHome.Core.Web.Extentions.Collection;
using PetHome.SharedKernel.Responses.ErrorManagement;
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
