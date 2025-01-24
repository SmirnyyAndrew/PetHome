using CSharpFunctionalExtensions;
using PetHome.Core.Extentions.Collection;
using PetHome.Core.Interfaces.FeatureManagment;
using PetHome.Core.Models;
using PetHome.Core.Response.Validation.Validator;
using PetHome.VolunteerRequests.Application.Database.Interfaces;
using PetHome.VolunteerRequests.Domain;

namespace PetHome.VolunteerRequests.Application.Features.Read.GetAllAdminVolunteerRequests;
public class GetAllAdminVolunteerRequestsUseCase
    : IQueryHandler<PagedList<VolunteerRequest>, GetAllAdminVolunteerRequestsQuery>
{
    private readonly IVolunteerRequestRepository _repository;

    public GetAllAdminVolunteerRequestsUseCase(
        IVolunteerRequestRepository repository)
    {
        _repository = repository;
    }


    public async Task<Result<PagedList<VolunteerRequest>, ErrorList>> Execute(
        GetAllAdminVolunteerRequestsQuery query, CancellationToken ct)
    {
        var volunteerRequests = await _repository.GetByAdminId(query.AdminId, ct);

        if (query.Status is not null)
            volunteerRequests = volunteerRequests.Where(d => d.Status == query.Status).ToList();

        PagedList<VolunteerRequest> volunteerRequestsPagedList = await volunteerRequests.AsQueryable()
            .ToPagedList(query.PageNum, query.PageSize, ct);

        return volunteerRequestsPagedList;
    }
}
